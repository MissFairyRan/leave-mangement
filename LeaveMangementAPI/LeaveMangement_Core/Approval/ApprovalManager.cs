﻿using LeaveMangement_Entity.Dtos.Approval;
using LeaveMangement_Entity.Model;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LeaveMangement_Core.Approval
{
    public class ApprovalManager
    {
        private KaoQinContext _ctx = new KaoQinContext();
        private ApprovalService _approvalService = new ApprovalService();
        
        public object AddApplication(AddApplicationDto addApplicationDto)
        {
            var result = new object();
            var application = _ctx.Apply.SingleOrDefault(a => a.WorkerId == addApplicationDto.WorkerId && 
            a.StartTime <= addApplicationDto.StartTime&&a.EndTime>=addApplicationDto.EndTime);
            int state = addApplicationDto.IsSubmit ? ApprovalHelper.DEFAULT_APPROVAL_STATE : 0;
            if (application != null)
                result = new
                {
                    isSuccess = false,
                    message = "您已有该时间段的假期！"
                };
            else
            {
                Apply newApply = new Apply()
                {
                    WorkerId = addApplicationDto.WorkerId,
                    DeparmentId = addApplicationDto.DeparmentId,
                    CompanyId = addApplicationDto.CompId,
                    Type1 = addApplicationDto.Type1,
                    Type2 = addApplicationDto.Type2,
                    Account = addApplicationDto.Account,
                    State = ApprovalHelper.DEFAULT_APPROVAL_STATE,
                    IsSubmit = addApplicationDto.IsSubmit,
                    StartTime = addApplicationDto.StartTime,
                    EndTime = addApplicationDto.EndTime,
                    CreateTime = DateTime.Now,
                };
                _ctx.Apply.Add(newApply);
                _ctx.SaveChanges();
                result = new
                {
                    isSuccess = true,
                    message = "申请已保存！"
                };
            }
            return result;
        }
        //获取提交申请列表
        public object GetApplicationList(string account)
        {
            var worker = _ctx.Worker.SingleOrDefault(w => w.Account.Equals(account));
            var list = (from apply in _ctx.Apply
                        join deparment in _ctx.Deparment on apply.DeparmentId equals deparment.Id
                        where apply.WorkerId == worker.Id&&apply.IsSubmit
                        select new { 
                            id = apply.Id,
                            workerName = worker.Name,
                            deparment = deparment.Name,
                            type = _approvalService.GetApplicationType(apply.Type1,apply.Type2),
                            state = apply.State,
                            stateName = _approvalService.GetStateName(apply.State),
                            startTime = apply.StartTime,
                            endTime = apply.EndTime,
                            createTime = apply.CreateTime,
                        }).ToList();
            return list;
        }
        public object GetApplicationById(int id)
        {
            var list = (from apply in _ctx.Apply
                        join worker in _ctx.Worker on apply.WorkerId equals worker.Id
                        join deparment in _ctx.Deparment on apply.DeparmentId equals deparment.Id
                        where apply.Id == id
                        select new
                        {
                            workerName = worker.Name,
                            deparment = deparment.Name,
                            account = apply.Account,
                            type = _approvalService.GetApplicationType(apply.Type1, apply.Type2),
                            state = apply.State,
                            stateName = _approvalService.GetStateName(apply.State),
                            startTime = apply.StartTime,
                            endTime = apply.EndTime,
                            remark = apply.Remark,
                            handerName = GetHanderName(apply.LeaderId),
                            handerTime = apply.HandleTime,
                            createTime = apply.CreateTime,
                        }).ToList();
            return list[0];
        }
        public object GetUnApplicationList(string account)
        {
            var worker = _ctx.Worker.SingleOrDefault(w => w.Account.Equals(account));
            var list = (from apply in _ctx.Apply
                        join deparment in _ctx.Deparment on apply.DeparmentId equals deparment.Id
                        where apply.WorkerId == worker.Id && !apply.IsSubmit
                        select new
                        {
                            id = apply.Id,
                            workerName = worker.Name,
                            deparment = deparment.Name,
                            type = _approvalService.GetApplicationType(apply.Type1, apply.Type2),
                            startTime = apply.StartTime,
                            endTime = apply.EndTime,
                            createTime = apply.CreateTime,
                        }).ToList();
            return list;
        }
        public object SubmitApplication(int id)
        {
            var result = new object();
            try
            {
                var application = _ctx.Apply.Find(id);
                application.IsSubmit = true;
                application.State = 1;
                _ctx.SaveChanges();
                result = new
                {
                    isSuccess = true,
                    message = "提交申请成功！",
                };
            }
            catch
            {
                result = new
                {
                    isSuccess = false,
                    message = "提交申请失败！",
                };
            }
            return result;
        }
        public object EditApplication(EditApplicationDto editApplicationDto)
        {
            var application = _ctx.Apply.Find(editApplicationDto.Id);
            var result = new object();
            try
            {
                application.Type1 = editApplicationDto.Type1;
                application.Type2 = editApplicationDto.Type2;
                application.Account = editApplicationDto.Account;
                application.StartTime = editApplicationDto.StartTime;
                application.EndTime = editApplicationDto.EndTime;
                application.IsSubmit = editApplicationDto.IsSubmit;
                _ctx.SaveChanges();
                result = new
                {
                    isSuccess = true,
                    message = "申请编辑成功！",
                };
            }
            catch
            {
                result = new
                {
                    isSuccess = false,
                    message = "申请编辑失败！",
                };
            }
            return result;
        }
        //获取处理申请人姓名
        public string GetHanderName(int? id)
        {
            try
            {
                return _ctx.Worker.Find(id).Name;
            }
            catch
            {
                return "";
            }            
        }

    }
}