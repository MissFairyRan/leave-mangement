<template>
<div>
  <div class="add-head">
      <router-link class="add-head-link" to="/login"><i class="el-icon-back"></i>返回</router-link>
      <div class="head-container">
          <div style="padding-top:15px;">
            <p class="head-title">JX人事考勤系统</p>
            <span class="add-company">添加公司</span>
          </div>
      </div>
  </div>  
  <div class="add-body">
      <div class="body-container">
          <div class="step-bar">
              <ol>
                  <li >
                      <div class="step-div step-bar-active"></div>
                  </li>
                  <li class="step-bar-active">
                      <div v-if="show === 'second'" class="step-div step-bar-active"></div>
                      <div v-else class="step-div"></div>
                  </li>
              </ol>
          </div>
          <div class="add-box">               
              <!-- <component v-bind:is="step"></component> -->
              <first-step v-if="show === 'first'" @change="changeShow"></first-step>
              <second-step v-if="show === 'second'" :result="result"></second-step>
          </div>
      </div>
  </div>
</div>
</template>
<script>
import './add.scss';
import FirstStep from './firstStep'
import SecondStep from './secondStep'
import { mapState } from 'vuex'
export default{
   components:{
       FirstStep,
       SecondStep
   },
   data(){
       return{  
           result:{},
           show:'first',       
       }
   },
   methods:{
       changeShow(data={}){
           this.result = data,
           this.show = 'second'
       }
   },
//    computed:mapState({
//            step: state=>state.addCompany.step
//        }),
}
</script>
<style lang="scss" scoped>

</style>
