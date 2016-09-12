using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using FundRaising.Models;

namespace FundRaising.Infrastructure
{
    
    public class CustomRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            using (var DB = new FundRaisingDBContext())
            {
                var user = DB.Students.SingleOrDefault(u => u.StudentID == username);
                if (user == null)
                    return false;
                var roles=DB.UserRoles.Where(x=>x.UserId==user.ID);
                var roleInfo = DB.Roles.Where(x => x.RoleName==roleName);
                
                return roles!= null && roleInfo!=null;
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var FundRaisingDBContext = new FundRaisingDBContext())
            {
              
                var user = FundRaisingDBContext.Students.SingleOrDefault(u => u.StudentID== username);
                
                if (user == null)
                {
                   
                        var adminuser = FundRaisingDBContext.Distributors.SingleOrDefault(x => x.UserName == username);
                        if(adminuser==null)
                        {
                            return new string[] { };
                        }
                        else
                        {
                            var roles = FundRaisingDBContext.UserRoles.Where(x => x.UserId == adminuser.userID).Select(x => x.RoleId).ToArray();
                            var selectedrole = (from role in FundRaisingDBContext.Roles where roles.Contains(role.RoleId) select role);
                            var roletoUser = selectedrole.Select(x => x.RoleName).ToArray();
                            return roles == null ? new string[] { } : roletoUser;
                        }
                   
                  
                   
                    

                    ///code to Give access to Role - Admin
                    ///--------------------------------------------------//////
                    // var users = FundRaisingDBContext.Distributors.SingleOrDefault(u => u.UserName == username);
                    //if(users==null)
                    //{
                    //    return new string[] { };
                    //}
                    //else
                    //{
                    //    var role = FundRaisingDBContext.UserRoles.Where(x => x.UserId == users.userID).Select(x => x.RoleId).ToArray();
                    //    var selectedroles = (from rol in FundRaisingDBContext.Roles where role.Contains(rol.RoleId) select rol);
                    //    var roletoUsers = selectedroles.Select(x => x.RoleName).ToArray();
                    //    //var rolesarray=allroles.ToArray();

                    //    return role == null ? new string[] { } : roletoUsers;
                    //}
                    ///--------------------------------------------------//////
                }
                else
                {
                    var roles = FundRaisingDBContext.UserRoles.Where(x => x.UserId == user.ID).Select(x => x.RoleId).ToArray();
                    var selectedrole = (from role in FundRaisingDBContext.Roles where roles.Contains(role.RoleId) select role);
                    var roletoUser = selectedrole.Select(x => x.RoleName).ToArray();
                    return roles == null ? new string[] { } : roletoUser;
                }
              
                //var rolesarray=allroles.ToArray();

                
                        return new string[] { };
                   
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            using (var FundRaisingDBContext = new FundRaisingDBContext())
            {
                return FundRaisingDBContext.Roles.Select(r => r.RoleName).ToArray();
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}