﻿using System;
using System.Collections.Generic;
using System.Text;
using LeaveMangement_Core.Approval;
using LeaveMangement_Entity.Dtos.Approval;

namespace LeaveMangement_Application.Approval
{
    public class ApprovalAppService : IApprovalAppService
    {
        private ApprovalManager _approvalManager = new ApprovalManager();
        public object AddApplication(AddApplicationDto addApplicationDto)
        {
            return _approvalManager.AddApplication(addApplicationDto);
        }
        public object GetApplicationList(GetApplicationListDto getApplicationListDto)
        {
            return _approvalManager.GetApplicationList(getApplicationListDto);
        }
        public object GetUnApplicationList(GetApplicationListDto getApplicationListDto)
        {
            return _approvalManager.GetUnApplicationList(getApplicationListDto);
        }
        public object GetApplicationById(int id)
        {
            return _approvalManager.GetApplicationById(id);
        }
        public object SubmitApplication(int id)
        {
            return _approvalManager.SubmitApplication(id);
        }
        public object EditApplication(EditApplicationDto editApplicationDto)
        {
            return _approvalManager.EditApplication(editApplicationDto);
        }
        public object DeleteApplicationById(int id)
        {
            return _approvalManager.DeleteApplicationById(id);
        }
        public int GetApprovalCount(string account, int compId)
        {
            return _approvalManager.GetApprovalCount(account, compId);
        }
    }
}
