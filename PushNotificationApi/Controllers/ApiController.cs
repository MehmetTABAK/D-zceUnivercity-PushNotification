using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PushNotificationEntites;

namespace PushNotificationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("sendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] FcmRequestModel model)
        {
            try
            {
                // FCM REST API URL'i
                string apiUrl = "https://fcm.googleapis.com/fcm/send";

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer AAAAuFLq1SY:APA91bEBuWdQrF65bbHk00atePEFb1zpspbgynguzs9X17FPLmgYK1rbCb5SIYjg7ytXLa8-6PEKzXmANDhqF96YTQ4zx-4tpf6wPc-Dxls10ZTix7jr2sjN5IWEfEam6lrOp9W_imon");

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Bildirim başarıyla gönderildi.");
                }
                else
                {
                    return BadRequest("Bildirim gönderme başarısız. HTTP kodu: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }
    }
}
