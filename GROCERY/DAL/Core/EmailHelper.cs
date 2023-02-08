using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class EmailHelper
    {
        public static void sendEmail(ref MailMessage mailMessage)
        {
            //The SmtpClient gets configuration from Web.Config
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mailMessage);
        }
    }
}