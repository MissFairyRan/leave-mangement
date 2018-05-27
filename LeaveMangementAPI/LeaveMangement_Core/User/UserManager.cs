﻿using LeaveMangement_Entity.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace LeaveMangement_Core.User
{
    public class UserManager
    {
        private KaoQinContext _ctx = new KaoQinContext();
        private UserService _userService = new UserService();
        public Worker Login(string account,string password)
        {
            //var admin = _ctx.AdminUser.SingleOrDefault(u => u.Account.Equals(account) && u.Password.Equals(password));
            //var result = new object();
            //if (admin != null)
            //    return admin;
            //else
            //{
            int[] positionIds = _ctx.Position.Where(u => u.Name.Equals("总经理") || u.Name.Equals("部门经理"))
                .Select(p => p.Id).ToArray();
            var manager = _ctx.Worker.SingleOrDefault(u => u.Account.Equals(account) && u.Password.Equals(password)
            && positionIds.Contains(u.PositionId));
            return manager ?? null;
            //}
        }
        public bool CreateCompanyAdmin(Company company)
        {
            string account = _userService.CreateAdmAccount();
            AdminUser user = new AdminUser()
            {
                Account = account,
                Name = company.Corporation,
                CompanyId = company.Id,
                Password = "123456",
                Status = "admin"
            };
            _ctx.AdminUser.Add(user);
            _ctx.SaveChanges();
            return _userService.SendEMail(company.Email,user);            
        }
    }
}
