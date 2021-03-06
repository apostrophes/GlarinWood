﻿using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Services
{
    public class EmailForForgetPassword:IEmailForForgetPassword
    {
        private IHostingEnvironment _env;

        public EmailForForgetPassword(IHostingEnvironment env)
        {
            _env = env;

        }

        public async Task SendEmailForgetPasswordAsync(string userId, string userName, string code)
        {
            try
            {

                var emailMessage = new MimeMessage();
                //string d = "/Utilites/LocalizedEmailTemplates/Contact.htm";
                //var webRoot = _env.WebRootPath;
                //string file = Path.Combine(webRoot, d);

                //var physicalProvider = _env.ContentRootFileProvider;
                //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی

                IFileProvider provider = new PhysicalFileProvider(_env.WebRootPath);
                /*   IDirectoryContents contents = provider.GetDirectoryContents("");*/ // the applicationRoot contents
                //استفاده از قالب موجود در ای پی پی دیتا

                IFileInfo fileInfo = provider.GetFileInfo("LocalizedEmailTemplates/ForgotPasswordUserEmail.htm"); // a file under applicationRoot



                string strEmailBody = File.ReadAllText(fileInfo.PhysicalPath);
                strEmailBody = strEmailBody
                                  .Replace("[USER_ID]", userId)
                                  .Replace("[USER_NAME]", userName)
                                  .Replace("[CODE]", code);



                //استفاده از متد سند کلاس میل مسیج


                //From Address
                string FromAddress = "Admin@Info.com";
                string FromAdressTitle = "Glarin Wood"; 
                //To Address
                string ToAddress = userName;
                string ToAdressTitle = "PasswordRecovery";
                string Subject = "بازیابی گذرواژه";
                string BodyContent = strEmailBody;

                //Smtp Server
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number
                int SmtpPortNumber = 587;


                emailMessage.From.Add(new MailboxAddress(System.Text.Encoding.UTF8, FromAdressTitle, FromAddress));
                emailMessage.To.Add(new MailboxAddress(System.Text.Encoding.UTF8, ToAdressTitle, ToAddress));
                emailMessage.Subject = Subject;
                emailMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                     await client.ConnectAsync(SmtpServer, SmtpPortNumber, false).ConfigureAwait(false);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(FromAddress, "zxcv@2vcxZ");
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);

                    //@ViewBag.Message = "Thank you for Contacting us ";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task SendEmailForPasswordAsync(string userId, string userName, string password)
        //{
        //    try
        //    {

        //        var emailMessage = new MimeMessage();
        //        //string d = "/Utilites/LocalizedEmailTemplates/Contact.htm";
        //        //var webRoot = _env.WebRootPath;
        //        //string file = Path.Combine(webRoot, d);

        //        //var physicalProvider = _env.ContentRootFileProvider;
        //        //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی

        //        IFileProvider provider = new PhysicalFileProvider(_env.WebRootPath);
        //        /*   IDirectoryContents contents = provider.GetDirectoryContents("");*/ // the applicationRoot contents
        //        //استفاده از قالب موجود در ای پی پی دیتا

        //        IFileInfo fileInfo = provider.GetFileInfo("LocalizedEmailTemplates/UserEmailVerification.htm"); // a file under applicationRoot



        //        string strEmailBody = File.ReadAllText(fileInfo.PhysicalPath);
        //        strEmailBody = strEmailBody
        //                          .Replace("[USER_ID]", userId)
        //                          .Replace("[USER_NAME]", userName)
        //                          .Replace("[PASSWORD]", password);



        //        //استفاده از متد سند کلاس میل مسیج


        //        //From Address
        //        string FromAddress = "Admin@Info.com";
        //        string FromAdressTitle = "Glarin Wood";
        //        //To Address
        //        string ToAddress = userName;
        //        string ToAdressTitle = "PasswordRecovery";
        //        string Subject = "تایید ثبت نام";
        //        string BodyContent = strEmailBody;

        //        //Smtp Server
        //        string SmtpServer = "smtp.gmail.com";
        //        //Smtp Port Number
        //        int SmtpPortNumber = 587;


        //        emailMessage.From.Add(new MailboxAddress(System.Text.Encoding.UTF8, FromAdressTitle, FromAddress));
        //        emailMessage.To.Add(new MailboxAddress(System.Text.Encoding.UTF8, ToAdressTitle, ToAddress));
        //        emailMessage.Subject = Subject;
        //        emailMessage.Body = new TextPart(TextFormat.Html)
        //        {
        //            Text = BodyContent
        //        };

        //        using (var client = new SmtpClient())
        //        {
        //            //client.LocalDomain = "Admin@Info.com"";
        //            await client.ConnectAsync(SmtpServer, SmtpPortNumber, false).ConfigureAwait(false);
        //            // Note: only needed if the SMTP server requires authentication
        //            // Error 5.5.1 Authentication 
        //            client.Authenticate(FromAddress, "zxcv@2vcxz");
        //            await client.SendAsync(emailMessage).ConfigureAwait(false);
        //            await client.DisconnectAsync(true).ConfigureAwait(false);

        //            //@ViewBag.Message = "Thank you for Contacting us ";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
