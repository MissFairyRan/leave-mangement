<template>
    <div class="index">
        <div class="index-title">
            <p>添加公告</p>
        </div>
        <div class="index-body" :model="notice">
            <el-form label-width="80px" class="demo-ruleForm">
                <el-form-item label="公告标题:" prop="title">
                    <el-input v-model="notice.title" style="width:50%;"></el-input>
                </el-form-item>
                <el-form-item label="公告类型:" prop="type">
                    <el-radio v-model="notice.type" label="1" border size="medium">公司公告</el-radio>
                    <el-radio v-model="notice.type" label="2" border size="medium">部门公告</el-radio>
                </el-form-item>
                <el-form-item label="公告内容:" prop="content">
                    <comp-editor v-model="notice.content" style="height:300px;" ref="notice_editor"></comp-editor>
                </el-form-item>
                <el-form-item style="display: flex;justify-content: flex-end;">
                    <el-button>重置</el-button>
                    <el-button type="primary" :loading="loading" @click="submitData">提交</el-button>
                </el-form-item>
            </el-form>
            
        </div>
        
    </div>
</template>
<script>
import CompEditor from '../../packages/components/editor'
import '../index.scss'
import {NoticeApi} from './api.js'
export default {
    components:{
        CompEditor
    },
    data(){
        return{
            loading:false,
            notice:{
                title:'',
                type:'',
                content:''
            }
        }
    },
    methods:{
        submitData(){
            this.loading = true
            var noticeDetail = this.$refs["notice_editor"].getEditorContent();
            this.notice.content = noticeDetail
            this.notice.type = this.notice.type === "1"?1:2
            NoticeApi.addNotice(this.notice).then(res=>{
                const type1 = res.data.isSuccess?'success':'error'
                this.$message({
                    type:type1,
                    message:res.data.message
                })
                this.loading = false
                this.$router.push({ path: '/notice'})
            })
        }
    }
}
</script>
