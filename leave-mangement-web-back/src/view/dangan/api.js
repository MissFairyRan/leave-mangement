import {server} from '@/tools/servers'

/**
 * 登录
 */
export class FileApi {
  // 获取公司信息
  static getCompany () {
    return server.connection('GET', 'api/File/GetCompanyInfo')
  }
  //获取到部门列表
  static getDeparmentList(data={}){
    return server.connection('POST','api/File/GetDeparmentList',data)
  }
  //添加部门时，获取经理的下拉列表
  static getWorkerList(){
    return server.connection('GET','api/File/GetWorkerList')
  }
  //单个添加部门
  static addSingleDep(data={}){
    return server.connection('POST','api/File/AddSingleDpearment',data)
  }
}