using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace FundRaising.App_Start
{
    [AllowAnonymous]
    public class EmailService : IIdentityMessageService
    {
        string _attachment="";
        public EmailService(string attachment="")
        {
            _attachment = attachment;
        }
        public Task SendAsync(IdentityMessage message)
        {
        //    MailMessage email = new MailMessage("nomail@Shopmergd.com", message.Destination);
        //    email.Subject = message.Subject;
        //    email.Body = message.Body;
        //    email.IsBodyHtml = true;
        //    string password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Montana@11"));
        //    var mailClient = new SmtpClient("smtp.sendgrid.net", 587) { Credentials = new NetworkCredential("infodatixUser",password), EnableSsl = false};
        //    return mailClient.SendMailAsync(email);
            //return Task.FromResult(0);



            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage("nomail@shopmerged.com", message.Destination);
            if (!string.IsNullOrEmpty(_attachment))
            {
                Attachment attach = new Attachment(_attachment);

                email.Attachments.Add(attach);
            }
             string[] splitedata = message.Subject.Split('<');
             email.Subject = splitedata[0];
             if (splitedata.Length > 1)
            {          
                email.Body = message.Body;
                email.IsBodyHtml = true;                               
               
                LinkedResource logo = new LinkedResource(splitedata[1]);
                logo.ContentId = "companyLogo";
                //message.Body=message.Body.Replace("<%image%>","<img src='cid:companyLogo' style='height: 100px;float: left;margin-right: 10px;margin-bottom: 8px;'/>");
                AlternateView av = AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html);
                av.LinkedResources.Add(logo);
                email.AlternateViews.Add(av);
                email.IsBodyHtml = true;
                var mailClient = new SmtpClient("smtp.gmail.com", 587) { Credentials = new NetworkCredential("testEac7@gmail.com", "popup$$1234"), EnableSsl = true };
                return mailClient.SendMailAsync(email);
            }
            else
            {
                
                email.Body = message.Body;
                AlternateView av = AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html);
                email.AlternateViews.Add(av);
                var mailClient = new SmtpClient("smtp.gmail.com", 587) { Credentials = new NetworkCredential("testEac7@gmail.com", "popup$$1234"), EnableSsl = true };
                return mailClient.SendMailAsync(email);
            }
            
            
        }


        
    }
}