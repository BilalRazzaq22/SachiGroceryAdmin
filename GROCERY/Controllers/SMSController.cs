using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class SMSController : Controller
    {
        BusinessController controller = new BusinessController();
        //
        // GET: /SMS/
        public ActionResult Bulk()
        {
            return View();
        }
        public JsonResult SendBulk(string msg)
        {
            string username = "chaarsu@bizsms.pk";
            string pass = "ch33rsuw9";
            string destinationnum = "";
            string masking = "Chaarsu";
            string text = "";
            string language = "English";
            int msgCount = 0;
            //start sending SMS on request
            if (msg != string.Empty)
            {
                msgCount = GenerateSMSAlert(masking, destinationnum, msg, username, pass);
            }

            //end sending SMS on request



            return Json(msgCount, JsonRequestBehavior.AllowGet);
        }
        #region Generate SMS

        public int GenerateSMSAlert(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            int count = 0;
            DataSet contacts = controller.getUserscContacts();
            DataTable cnt = contacts.Tables[0];

            //SMS sms = new SMS();
            if (contacts.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rowContact in cnt.Rows)
                {
                    toNumber = rowContact["MOBILE_NO"].ToString();
                    if (toNumber.Length == 12 && toNumber.Substring(0, 2).Equals("92"))
                    {
                        count++;
                    }
                    SendSMS(Masking, toNumber, MessageText, MyUsername, MyPassword);
                    //DataRow r = sms.NewRow();
                    //r[Entities.SMS.HOSPITAL_CONTACT_ID] = rowContact[Entities.HOSPITAL_CONTACTS.HOSPITAL_CONTACT_ID];
                    //r[Entities.SMS.SENDER] = SessionManager.GetUserSession();
                    //r[Entities.SMS.TEXT] = MessageText;
                    //r[Entities.SMS.RESPONSE] = jsonResponse;
                    //r[Entities.SMS.SENT_ON] = DateTime.Now;
                    //r[Entities.SMS.SMS_TYPE_ID] = 1;
                    //r[Entities.SMS.CYLINDER_REQUEST_ID] = cylinderRequestId;
                    //sms.Rows.Add(r);

                }
            }
            return count;
            //           int smsSaved = bridge.saveSMS(sms);

        }
        #endregion

        #region Send SMS

        public static string SendSMS(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            //string URI = @"http://api.bizsms.pk/api-send-branded-sms.aspx?username=" + MyUsername + "&pass=" + MyPassword +
            //    "&text=" + MessageText + "&masking=" + Masking + "&destinationnum=" + toNumber + "&language=English";

            //WebRequest req = WebRequest.Create(URI);
            //WebResponse resp = req.GetResponse();
            //var sr = new System.IO.StreamReader(resp.GetResponseStream());
            //return sr.ReadToEnd().Trim();

            SmsApiService.QuickSMSResquest quickSMSResquest = new SmsApiService.QuickSMSResquest()
            {
                loginId = MyUsername,
                loginPassword = MyPassword,
                Destination = toNumber,
                Mask = Masking,
                Message = MessageText,
                ShortCodePrefered = "n",
                UniCode = "0"
            };

            SmsApiService.BasicHttpBinding_ICorporateCBS client = new SmsApiService.BasicHttpBinding_ICorporateCBS();
            string message = client.QuickSMS(quickSMSResquest);
            return message;

        }
        #endregion
    }
}