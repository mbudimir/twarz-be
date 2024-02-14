using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using FirebaseAdmin.Auth;
using Twarz.API.Domains;

namespace Twarz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageRequest request)
        {

            string url = "https://fcm.googleapis.com/fcm/send";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAJwZdmw4:APA91bFkHUrFY2dhavJZ7LZ2TeWAkfmrhZbbGnTvGMDvYrcfVGdGX-BR_CoT0tRavUF-MgLRIv30A3bR1sRyFpzAxFk3-g2mS5XxGmjNEWhjfAsjSB2iHyErKTLuBXeY2Evcx9JwGkPi");
                var registrationToken = "cU7ccZXeS8eUAHa_oDJxv_:APA91bGuzH7Q9JiSchzbgPODmF7mYYhbwj1Npin5C7IkCu5nhOTK_orZUP9VuvFbr-GXD-6hgq322yzJvx-W3pjFuDsJPqLDhhvnET-NogiCfm3cQhVYhlr5U-RvnegIh65rsAnpBgsQ";


                //var pushNotificationRequest = new Message()
                //{
                //    Notification = new Notification
                //    {
                //        Title = request.Title,
                //        Body = request.Body,
                //    },
                //    Data = new Dictionary<string, string>()
                //    {
                //        { "score", "850" },
                //        { "time", "2:45" },
                //    },
                //    Token = registrationToken
                //};


                var pushNotificationRequest = new PushNotificationRequest
                {
                    notification = new NotificationMessageBody
                    {
                        title = request.Title,
                        body = request.Body
                    },
                    // data = androidNotificationObject,
                    registration_ids = new List<string> { request.DeviceToken }
                };

                string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok("Message sent successfully!");
                }
                else
                {
                    // There was an error sending the message
                    throw new Exception("Error sending the message.");
                }
            }
            //var message = new Message()
            //{
            //    Notification = new Notification
            //    {
            //        Title = request.Title,
            //        Body = request.Body,
            //    },
            //    Data = new Dictionary<string, string>()
            //    {
            //        { "score", "850" },
            //        { "time", "2:45" },
            //    },
            //    Token = registrationToken
            //};

            //var messaging = FirebaseMessaging.DefaultInstance;
            //var result = await messaging.SendAsync(message);

            //if (!string.IsNullOrEmpty(result))
            //{
            //    // Message was sent successfully
            //    return Ok("Message sent successfully!");
            //}
            //else
            //{
            //    // There was an error sending the message
            //    throw new Exception("Error sending the message.");
            //}
        }
    }

    public class PushNotificationRequest
    {
        public List<string> registration_ids { get; set; } = new List<string>();
        public NotificationMessageBody notification { get; set; }
        public object data { get; set; }
    }

    public class NotificationMessageBody
    {
        public string title { get; set; }
        public string body { get; set; }
    }
}
