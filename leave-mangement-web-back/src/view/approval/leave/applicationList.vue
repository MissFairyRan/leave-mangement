<template>
    <div class="index">
        <div class="index-title">
            <p>已提交申请</p>
        </div>
        <div class="index-body">
            <div class="index-body-title">
                <div style="display:flex;">
                    <p>共 <span>{{totalCount}}</span> 条提交的申请</p>
                    <router-link to="/leave/addApplication/-1" class="link"><i class="el-icon-plus"></i>添加申请</router-link>
                </div>
                <el-input style="width:30%" placeholder="请输入内容" v-model="query" class="input-with-select">
                    <el-button slot="append" @click="handleChangeQuery()">搜索</el-button>
                </el-input>
            </div>
            <el-table :data="tableData" :header-cell-style="headerCellStyle" border style="width: 100%">
                <el-table-column align="center" prop="id" label="序号" width="80">
                    <template slot-scope="scope">
                            {{scope.row.id}}
                    </template>
                </el-table-column>
                <el-table-column align="center" prop="workerName" label="员工名称" width="100">
                </el-table-column>
                <el-table-column align="center" prop="type" label="请假类型" width="120">
                </el-table-column>
                <el-table-column align="center" label="申请状态" width="100">
                    <template slot-scope="scope">
                        <p v-if="scope.row.state===1" class="approval-state unlook">{{scope.row.stateName}}</p>
                        <p v-if="scope.row.state===2" class="approval-state agree">{{scope.row.stateName}}</p>
                        <p v-if="scope.row.state===3" class="approval-state refuse">{{scope.row.stateName}}</p>
                    </template>
                </el-table-column>
                <el-table-column align="center" prop="startTime" label="开始时间">
                    <template slot-scope="scope">
                        {{scope.row.startTime | formatDate}}
                    </template>
                </el-table-column>
                <el-table-column align="center" prop="endTime" label="结束时间">
                    <template slot-scope="scope">
                        {{scope.row.endTime | formatDate}}
                    </template>
                </el-table-column>
                <el-table-column align="center" prop="createTime" label="创建时间">
                </el-table-column>
                <el-table-column align="center" label="是否结束">
                    <template slot-scope="scope">
                        <p v-if="getIsEnd(scope.row.endTime)">已结束</p>
                        <p v-else>-</p>
                    </template>
                </el-table-column>                
                <el-table-column align="center" label="操作" width="80">
                    <template slot-scope="scope">
                        <el-dropdown>
                            <i class="el-icon-menu"></i>
                            <el-dropdown-menu slot="dropdown">
                                <el-dropdown-item>
                                 <el-button type="text" size="small" @click="handleLook(scope.row.id)" icon="el-icon-view">查看</el-button>
                                </el-dropdown-item>
                                <el-dropdown-item v-if="getIsEnd(scope.row.endTime)">
                                 <el-button @click="handleRevoke(scope.row.id)" style="color:red;" type="text" size="small" icon="el-icon-delete">销假</el-button>
                                </el-dropdown-item>
                            </el-dropdown-menu>
                        </el-dropdown>
                    </template>
                </el-table-column>
            </el-table>
            <el-pagination 
            style="padding-top:10px"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
            :current-page="currentPage"
            :page-sizes="[100, 200, 300, 400]"
            :page-size="pageSize"
            layout="total, sizes, prev, pager, next, jumper"
            :total="totalCount">
            </el-pagination>
        </div>
        <el-dialog title="申请详情" :visible.sync="dialogVisible" :before-close="handleClose">
         <comp-look @closeForm='handleClose' @close='closeForm' :id="applicationId" ref="compForm"></comp-look>
        </el-dialog>
    </div>
</template>
<script>
    import '../../index.scss'
    import '../approval.scss'
    import CompLook from './applicationView'
    import { ApprovalApi } from "../api.js"
    export default {
        components:{
            CompLook
        },
        data() {
            return {
                tableData: [],
                headerCellStyle: {
                    backgroundColor: '#f2f2f2',
                    fontSize: '14px',
                    color: '#434343',
                    fontWeight: 400
                },
                currentPage: 1,
                pageSize: 20,
                query: '',
                totalCount: 0,
                dialogVisible: false,
                applicationId:0,
                nowData:''
            }
        },
        mounted(){
            this.loadData()
        },
        methods: {
            loadData() {
                this.nowData = new Date().getTime()
                const params={
                    currentPage: this.currentPage,
                    currentPageSize: this.pageSize,
                    query: this.query,
                }
                ApprovalApi.getApplicationList(params).then(res=>{
                    this.totalCount = res.data.count
                    this.tableData = res.data.data
                })
            },
            handleSizeChange(val) {
                this.pageSize = val;
                this.loadData();
            },
            handleCurrentChange(val) {
                this.currentPage = val;
                this.loadData();
            },
            getIsEnd(endTime){
                if(endTime > this.nowData){
                    return false
                }else{
                    return true
                }
            },
            //点击搜索
            handleChangeQuery() {
                this.loadData();
            },
            //点击查看
            handleLook(id){
                this.applicationId = id
                this.dialogVisible = true                
            },
            //点击销假
            handleRevoke(id){
                this.$confirm('您确定销假吗?', '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        ApprovalApi.revokeApplication(id).then(res => {
                            this.loadData()
                            const type1 = res.data.isSuccess? 'success':'error'
                            this.$message({
                                type: type1,
                                message: res.data.message
                            });                            
                        })
                    }).catch(() => {
                        this.$message({
                            type: 'info',
                            message: '已取消删除'
                        });
                    });
            },
            //关闭弹框
            handleClose(){
                this.dialogVisible = false
            },
            closeForm(val){
                this.loadData()
                this.dialogVisible = val
            }
        },
        filters: {
            formatDate: function (value) {
                let date = new Date(value);
                let y = date.getFullYear();
                let MM = date.getMonth() + 1;
                MM = MM < 10 ? ('0' + MM) : MM;
                let d = date.getDate();
                d = d < 10 ? ('0' + d) : d;
                return y + '-' + MM + '-' + d;
                }
            }
    }
</script>
<style lang="scss" scoped>
    .link {
        padding: 0px 0px 0px 10px;
        font-size: 14px;
        color: #409eff;
        text-decoration: none;
    }
</style>

