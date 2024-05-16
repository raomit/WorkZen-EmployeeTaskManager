using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WorkZen.Models
{
    public class WorkZenUserRoleProvider : RoleProvider
    {
        private String[] UserRole;
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string UserEmail)
        {
            if (this.UserRole == null)
            {
                using (MitRaoEntities _DbContext = new MitRaoEntities())
                {
                    this.UserRole = (from user in _DbContext.Employees
                                     join Department in _DbContext.Departments
                                     on user.departmentId equals Department.id
                                     where user.email == UserEmail
                                     select Department.name).ToArray();
                    return this.UserRole;
                }
            }
            else
            {
                return this.UserRole;
            }
            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}