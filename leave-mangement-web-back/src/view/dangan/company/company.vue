<template>
<el-card style="width:82%" class="box-card">
    <el-form v-model="companyInfo" label-width="100px">
        <el-form-item label="公司名称:">
            <el-input v-model="companyInfo.name" :readonly="readonly" style="width:40%;"></el-input>
        </el-form-item>
        <el-form-item label="成立时间:">
            <el-input v-model="companyInfo.createTime" :readonly="true" style="width:40%;"></el-input>
        </el-form-item>
        <el-form-item label="公司地址：">
            <el-input v-model="companyInfo.address" :readonly="readonly"></el-input>
        </el-form-item>
        <el-form-item label="公司法人:">
            <el-input v-model="companyInfo.corporation" :readonly="readonly" style="width:40%;"></el-input>
        </el-form-item>
        <el-form-item label="固定电话:">
            <el-input v-model="companyInfo.cellphoneNumber" :readonly="readonly" style="width:40%;"></el-input>
        </el-form-item>
        <el-form-item label="部门数量:">
            <el-input v-model="companyInfo.deparmentCount" :readonly="true" style="width:40%;"></el-input>
        </el-form-item>
        <el-form-item label="员工数量:">
            <el-input v-model="companyInfo.wokerCount" :readonly="true" style="width:40%;"></el-input>
        </el-form-item>
        <el-form-item>
            <el-button v-if="showSave != 'show'" @click="handleEdit()" type="primary" size="mini">编辑</el-button>
            <el-button v-if="showSave === 'show'" @click="handleExit()" size="mini">取消</el-button>
            <el-button v-if="showSave==='show'" @click="handleSave()" :loading="loading" type="primary" size="mini">保存</el-button>
        </el-form-item>
    </el-form>
</el-card>
</template>
<script>
import {FileApi} from '../api.js'
export default {
    data(){
        return{
           companyInfo:{},
           showSave:'hide',
           readonly:true,
           loading:false,
        }
    },
    created(){
        this.getCompanyInfo()
    },
    mounted() {
            this.getCompanyInfo()
        },
        methods: {
            getCompanyInfo() {
                FileApi.getCompany().then(res => {
                    this.companyInfo = {
                        ...res.data
                    }
                    this.companyInfo.createTime = this.formatDate(this.companyInfo.createTime)
                })
            },
            handleEdit(){
                this.showSave = 'show'
                this.readonly = false
            },
            handleExit(){
                this.showSave = 'hide',
                this.getCompanyInfo()
            },
            handleSave(){
                this.loading = true
                const params={
                    compId: this.companyInfo.id,
                    name: this.companyInfo.name,
                    address: this.companyInfo.address,
                    corporation: this.companyInfo.corporation,
                    cellphoneNumber: this.companyInfo.cellphoneNumber
                }
                FileApi.editCompany(params).then(res=>{
                    const type1 = res.data.isSuccess?'success':'error'
                    this.$message({
                        type:type1,
                        message:res.data.message
                    })
                    this.showSave = 'hide',
                    this.getCompanyInfo();
                })
            },
            formatDate (value) {
                let date = new Date(value);
                let y = date.getFullYear();
                let MM = date.getMonth() + 1;
                MM = MM < 10 ? ('0' + MM) : MM;
                let d = date.getDate();
                d = d < 10 ? ('0' + d) : d;
                return y + '-' + MM + '-' + d;
                }
        },
}
</script>

