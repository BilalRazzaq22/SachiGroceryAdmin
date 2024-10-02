using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace GROCERY.Notifications
{
    public class FCMPushNotification
    {
        //generic message sender ID 981605220744
        public FCMPushNotification()
        {
            // TODO: Add constructor logic here
        }

        public bool Successful
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }
        public Exception Error
        {
            get;
            set;
        }

        public FCMPushNotification SendNotification(string title, string message, string topic, string senderId, string deviceId)
        {
            FCMPushNotification result = new FCMPushNotification();
            try
            {
                result.Successful = true;
                result.Error = null;
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/private_key.json");
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(filePath),
                        ProjectId = "chaarsu-ef408"
                    });
                }

                var msg = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "body", message },
                        { "title", title },
                        { "to", deviceId },
                        { "priority", "high" }
                    },
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = message
                    },
                    Token = deviceId,
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High
                    },
                    Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            ContentAvailable = true,
                            Alert = new ApsAlert()
                            {
                                Title = title,
                                Body = message
                            }
                        }
                    }
                };

                if (FirebaseMessaging.DefaultInstance != null)
                {
                    string responsess = FirebaseMessaging.DefaultInstance.SendAsync(msg).Result;
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }
            return result;
        }
    }
}