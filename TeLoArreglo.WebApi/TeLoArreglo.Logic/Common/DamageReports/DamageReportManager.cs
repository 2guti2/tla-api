using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common.DamageReports
{
    public class DamageReportManager : IDamageReportManager
    {
        private const string ServerKey = "AAAAUOUJtD0:APA91bGw74BltDiAeeFKc32PUgYpwtQ182rtAUEQDNIe-gMa-fdibk-DLIb8T342588Eq0qvsjK2AdXLqGspTZbfFMuK-Ts6uEU3F_-PCYX3XFI6t-NX_1hNnVw6nFNy1cPKiz69mhMJ";
        private const string SenderId = "347440002109";

        public void SendDamageRepairedNotification(List<Device> devices)
        {
            SendNotificationToDevices(devices, "Damage Repaired", "One of your damages has been repaired");
        }

        public void SendDamageAcceptedNotification(List<Device> crewDevices)
        {
            SendNotificationToDevices(crewDevices, "Damage Accepted", "A new damage has been accepted and it's ready to be repaired");
        }

        public void SendNotificationToDevices(List<Device> devices, string title, string body)
        {
            foreach (Device device in devices)
            {
                string deviceId = device.DeviceToken;

                WebRequest tRequest = BuildRequest();

                string json = BuildJsonRequest(deviceId, title, body);

                Byte[] byteArray = AddHeaders(tRequest, json);

                SendRequest(tRequest, byteArray);
            }
        }

        public void SendRequest(WebRequest tRequest, Byte[] byteArray)
        {
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            string str = sResponseFromServer;
                        }
                    }
                }
            }
        }

        public Byte[] AddHeaders(WebRequest tRequest, string json)
        {
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            tRequest.Headers.Add($"Authorization: key={ServerKey}");
            tRequest.Headers.Add($"Sender: id={SenderId}");
            tRequest.ContentLength = byteArray.Length;

            return byteArray;
        }

        private string BuildJsonRequest(string deviceId, string title, string body)
        {
            var data = BuildPayload(deviceId, title, body);

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(data);
        }

        public WebRequest BuildRequest()
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";

            return tRequest;
        }

        public object BuildPayload(string deviceId, string titleContent, string bodyContent)
        {
            return new
            {
                to = deviceId,
                notification = new
                {
                    body = bodyContent,
                    title = titleContent,
                    sound = "Enabled"
                }
            };
        }
    }
}
