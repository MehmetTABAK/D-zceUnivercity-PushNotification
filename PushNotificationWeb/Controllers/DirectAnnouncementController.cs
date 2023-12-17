using Microsoft.AspNetCore.Mvc;
using PushNotificationDataAccess;
using PushNotificationEntites;
using PushNotificationWeb.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using PushNotificationDbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PushNotificationWeb.Controllers
{
    public class DirectAnnouncementController : Controller
    {
        PushNotificationContext context = new(); //PushNotificationContext, veritabanı işlemleri için kullanılacak olup "context" adında bir değişken oluşturulur ve bu değişken, PushNotificationContext sınıfının bir örneğiyle doldurulur.Yani, PushNotificationContext sınıfından bir nesne oluşturularak, veritabanı işlemleri için gerekli bağlam hazırlanmış olur.

        [Authorize]
        [HttpGet]
        public IActionResult SendNotify() //Admin panelinden paylaşım yapmadan direkt olarak duyuru gönderme işlemini yapmamız için gereken bor formu açmamızı sağlar.
        {
            return View(); //Direkt duyuru göndermek için kullanılacak formun bulunduğu View'ı gösterir.
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendNotify(FcmRequestModel model, int roleId) //Admin panelinden paylaşım yapmadan direkt olarak duyuru gönderme işlemini yapmamız için gereken bor formu doldurduktan sonra gönderebilmemizi sağlar.
        {
            try
            {
                string apiUrl = "https://localhost:7051/api/Api/sendNotification"; //Gönderme işlmeminin yapılacağı Apinin urlsi.

                var usersInRole = context.UserRoleRelation.Where(ur => ur.RoleId == roleId).Select(ur => ur.User.DeviceToken).ToList(); //Role bağlı kullanıcıların cihaz token'ları çekilir.Mesela öğrenci seçilmiş ise tüm öğrencilerin cihaz tokenleri alınır.

                foreach (var deviceToken in usersInRole) //Her bir kullanıcı için FCM isteği yapılır.
                {
                    var requestData = new FcmRequestModel //FCM isteği için gerekli model oluşturulur.
                    {
                        To = deviceToken, //Bildirimin gönderileceği hedef cihazın FCM cihaz kimliği (deviceToken) belirtilir.
                        Notification = new NotificationDataModel //Bildirimin içeriği için NotificationDataModel nesnesi oluşturulur.
                        {
                            Title = model.Notification.Title, //Bildirimin başlığı, kullanıcının yazdığı başlık ile doldurulur.
                            Body = model.Notification.Body //Bildirimin başlığı, kullanıcının yazdığı içerik ile doldurulur.
                        }
                    };

                    HttpClient httpClient = new HttpClient(); //HTTP isteği yapmak için HttpClient nesnesi oluşturulur.
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData); //Model JSON formatına dönüştürülür.
                    var content = new StringContent(json, Encoding.UTF8, "application/json"); //JSON içeriği ile HTTP isteği oluşturulur.

                    var response = await httpClient.PostAsync(apiUrl, content); //HTTP isteği gönderilir ve cevap alınır.

                    if (response.IsSuccessStatusCode) //Veri başarıyla gönderildiyse
                    {
                        ViewBag.ResponseMessage = "Veri başarıyla gönderildi."; //Başarılı mesajı gösterilir.
                    }
                    else //Eğer istek başarısız olursa
                    {
                        ViewBag.ResponseMessage = "Veri gönderme başarısız. HTTP kodu: " + response.StatusCode; //Hata mesajı gösterilir.
                    }
                }
            }
            catch (Exception ex) //Hata oluşursa
            {
                ViewBag.ResponseMessage = "Hata oluştu: " + ex.Message; //Hata mesajı gösterilir.
            }

            return RedirectToAction("Index","Announcement"); //İşlem tamamlandıktan sonra Announcement kontrolcüsünün Index metodu çağrılarak Index sayfasına yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public IActionResult SendEmail() //Admin panelinden e-posta gönderme işlemini yapmamız için gereken bor formu açmamızı sağlar.
        {
            return View(); //Email göndermek için kullanılacak formun bulunduğu View'ı gösterir.
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendEmail(EmailModel model) //Admin panelinden e-posta gönderme işlemini yapmamız için gereken bor formu doldurduktan sonra gönderebilmemizi sağlar.
        {
            var smtpClient = new SmtpClient("smtp.gmail.com") //Yeni bir SmtpClient (SMTP istemcisi) nesnesi oluşturulur. Gmail için "smtp.gmail.com" adresi kullanılır.
            {
                Port = 587, //SMTP sunucu bağlantı portu belirlenir (Gmail için genellikle 587 kullanılır).
                Credentials = new NetworkCredential("mtabak229@gmail.com", "dkpk ehhq ijvh lugb"), //SMTP sunucu kimlik bilgileri belirlenir (Gmail hesap bilgileri kullanılır).
                EnableSsl = true, //Güvenli bağlantı (SSL) kullanılır.
            };

            var mailMessage = new MailMessage //Yeni bir MailMessage (e-posta mesajı) nesnesi oluşturulur.
            {
                From = new MailAddress(model.Sender), //E-posta gönderen adresi belirlenir.
                Subject = model.Subject, //E-posta konusu belirlenir.
                Body = model.Message, //E-posta mesajı belirlenir.
                IsBodyHtml = false, //E-posta mesajının HTML formatında olup olmadığı belirlenir (false ise metin tabanlıdır).
            };

            mailMessage.To.Add(model.Recipient); //E-posta alıcısı belirlenir ve MailMessage nesnesine eklenir.

            try
            {
                smtpClient.Send(mailMessage); //Oluşturulan e-posta mesajı, belirlenen SMTP istemcisi aracılığıyla gönderilir.
                ViewBag.Message = "E-posta gönderildi."; //Eğer e-posta gönderme işlemi başarılı ise, bir mesaj ViewBag aracılığıyla View'a iletilir.
            }
            catch (Exception ex) //Eğer e-posta gönderme işlemi sırasında bir hata oluşursa
            {
                ViewBag.Message = "E-posta gönderme hatası: " + ex.Message; //Hata mesajı ViewBag aracılığıyla View'a iletilir.
            }

            return View(); //İşlem tamamlandıktan sonra View'a yönlendirme yapılır.
        }
    }
}
