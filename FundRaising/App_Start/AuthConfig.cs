using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using FundRaising.Models;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;
using WebMatrix.Data;

namespace FundRaising
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}

    //     private class SimpleMembershipInitializer
    //{
    //          private static SimpleMembershipInitializer _initializer;
    //private static object _initializerLock = new object();
    //    public SimpleMembershipInitializer()
    //    {
            
    //        System.Data.Entity.Database.SetInitializer<UsersContext>(null);

    //        try
    //        {
    //            using (var context = new UsersContext())
    //            {
    //                if (!context.Database.Exists())
    //                {
    //                    // Create the SimpleMembership database without Entity Framework migration schema
    //                    ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
    //                }
    //            }

    //            WebSecurity.InitializeDatabaseConnection("DBConnection", "webpages_UsersInRoles","UserId", "", autoCreateTables: true);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
    //        }
    //    }
    //}
//}
 