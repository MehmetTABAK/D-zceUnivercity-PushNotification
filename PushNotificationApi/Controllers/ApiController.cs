using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PushNotificationEntites;

// Bu katman siteden alınan (PushNotificationEntities katmanından alınan) bilgilerle birlikte firebase sitesine gitmemizi ve oradan da mobil cihazımıza bildirim gönderme işlemini gerçekleştirmektedir.

namespace PushNotificationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("sendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] FcmRequestModel model) //PushNotificationEntites katmanından FcmRequestModel için model isminde bir nesne oluşturuyoruz.
        {
            try
            {
                string apiUrl = "https://fcm.googleapis.com/fcm/send"; //Apimizin Url'ini belirtiyoruz.

                var httpClient = new HttpClient(); //HTTP üzerinden iletişim kurmak için HttpClient sınıfından bir nesne oluşturuyoruz.

                //Firebase bağlantısı kurmak için iletişime geçilecek firebase ile ilgili bilgileri giriyoruz.
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer AAAAuFLq1SY:APA91bEBuWdQrF65bbHk00atePEFb1zpspbgynguzs9X17FPLmgYK1rbCb5SIYjg7ytXLa8-6PEKzXmANDhqF96YTQ4zx-4tpf6wPc-Dxls10ZTix7jr2sjN5IWEfEam6lrOp9W_imon");

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(model); //Oluşturduğumuz model nesnesini JSON formatına dönüştürüyoruz.

                var content = new StringContent(json, Encoding.UTF8, "application/json"); //JSON formatındaki veriyi HTTP isteği içinde taşıyoruz.

                var response = await httpClient.PostAsync(apiUrl, content); //İşlemeler yapılınca verdiğimiz url'e verilen bilgilerle birlikte gidiyoruz.

                if (response.IsSuccessStatusCode) //İşlem başarılı olursa
                {
                    return Ok("Bildirim başarıyla gönderildi.");
                }
                else //İşlem başarısız olursa
                {
                    return BadRequest("Bildirim gönderme başarısız. HTTP kodu: " + response.StatusCode); //Başarısız olduğunu ve olma sebebini yazdırıyoruz.
                }
            }
            catch (Exception ex) //Farklı bir hata olursa
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message); //Hata oluştuğunu ve hata kodunu yazdırıyoruz.
            }
        }
    }
}
