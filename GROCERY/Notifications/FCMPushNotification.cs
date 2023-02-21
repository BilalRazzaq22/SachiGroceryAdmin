using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            if (deviceId.Contains("dsQLdD5ZRqOZ-gadh69KXf:APA91bELHFjBbuWwiafldWLpM5ot7kEt1seK0rBOz58BSnQHHL8rl5wjPCp55OOIKh3y3BRvaLnU_aGG4YzdgLgI8W_7_8oI7guaBeVE4T4G57Jq88rNQNR_JUN-IJFf6xg5sOneQI3c"))
            {

            }

            FCMPushNotification result = new FCMPushNotification();
            try
            {
                result.Successful = true;
                result.Error = null;
                // var value = message;
                var requestUri = "https://fcm.googleapis.com/fcm/send";

                WebRequest webRequest = WebRequest.Create(requestUri);
                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", "AIzaSyCY6eo8E8n-VwXNJ-18w0t_1IFiMDG-t2k"));
                webRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                webRequest.ContentType = "application/json";

                //                {
                //                    "data": {
                //                        "body" : "test",
                //     "title": "Title of Your Notification in Title"
                //                    },
                //    "to": "dsQLdD5ZRqOZ-gadh69KXf:APA91bELHFjBbuWwiafldWLpM5ot7kEt1seK0rBOz58BSnQHHL8rl5wjPCp55OOIKh3y3BRvaLnU_aGG4YzdgLgI8W_7_8oI7guaBeVE4T4G57Jq88rNQNR_JUN-IJFf6xg5sOneQI3c",
                //    "priority": "high"
                //}


                var response = new
                {
                    // to = YOUR_FCM_DEVICE_ID, // Uncoment this if you want to test for single device
                    //to = "dXjImqLLBOQ:APA91bHawHcRVgSxVuCzqomzOxU2DRLGGkHgRlPc0XuALZSbWyXCWujZhUgfQQXQ0IulqS1hroPdrmTLrRrIE40hCYyxiyzFsGFfW4V1aZsV0nsB0O8qzz39z0iN8ntww1D4pYWNuiyw",
                    //to = "ckO-sb0HwWY:APA91bE97NIFvohOpy7cVFMLKT3dbbpQMOPf3c4jJpAn3LD4bxqmM2aY2ebT5YRwdzDCrck3PLjU2RIRtzd7lE-FvG_hZRASy-02BxIs1v2NmT8Q6LVC_Lu50H_cMmsxgqE9xvM_nfFt",
                    //to = "cfyNOXEPar0:APA91bEeFgEtBcRIcDEae3QDaxcoOA8FZF-9KwI4q6n6DwrcOprOSRlBcz4jZbKo093ll6KQ7by-rHFo3qw6_nEisLU0RnQHttWH_RiEh2JnYZ4wLTJiUE2hHEvVWD_afxvvKiWHPm_h",
                    data = new
                    {
                        body = message,
                        title = title
                    },
                    to = deviceId,
                    priority = "high"
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(response);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                webRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
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
        //public void SendMessageNotification()
        //{
        //    FCMPushNotification fcmPush = new FCMPushNotification();
        //    fcmPush.SendNotification("Veggiefy", "THIS IS TEST MESSAGE", "news");
        //}
    }
}