using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationDbEntities;
using PushNotificationDbEntities.DTOs;
using PushNotificationEntites;
using System.Security.Policy;
using System.Text;

namespace PushNotificationWeb.Controllers
{
    public class AnnouncementController : Controller
    {
        PushNotificationContext context = new(); //PushNotificationContext, veritabanı işlemleri için kullanılacak olup "context" adında bir değişken oluşturulur ve bu değişken, PushNotificationContext sınıfının bir örneğiyle doldurulur.Yani, PushNotificationContext sınıfından bir nesne oluşturularak, veritabanı işlemleri için gerekli bağlam hazırlanmış olur.

        [Authorize]
        [HttpGet]
        public IActionResult Index() //Admin paneindeki duyuruları listelememizi sağlar.
        {
            List<Announcement> announcements = context.Announcements.ToList(); //Veritabanındaki Announcements tablosundaki tüm duyuruları çekip bir liste olarak alır.
            return View(announcements); //Duyuruların listesini View'a gönderir.
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddNotification() //Admin panelinden duyuru ekleme için gerekli boş formun açılmasını sağlar.
        {
            return View(); //Duyuru eklemek için kullanılacak formun bulunduğu View'ı gösterir.
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddNotification(Announcement model) //Admin panelinden duyuru ekleme için gerekli boş formun doldurulduktan sonra kaydedilmesini sağlar.
        {
            if (string.IsNullOrEmpty(model.PhotoUrl)) //Eğer fotoğraf URL'si belirtilmemişse
            {
                model.PhotoUrl = "https://i.hizliresim.com/b5t2vap.jpg"; //buradaki belirttiğim URL atanır.
            }

            Announcement anno = new() //Yeni duyuru oluşturulur.
            {
                Title = model.Title, //Duyurunun başlığını alır.
                Body = model.Body, //Duyurunun içeriğini alır.
                PhotoUrl = model.PhotoUrl, //Duyurunun resim urlsini alır.
            };

            context.Add(anno); //Duyuruyu veritabanına ekler.
            context.SaveChanges(); //Değişiklikler (duyuru) veritabanına kaydedilir.

            return RedirectToAction("Index"); //Index sayfasına yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id) //Admin paneindeki seçilen duyuruyu silmemmizi sağlar.
        {
            var anno = await context.Announcements.FirstOrDefaultAsync(w => w.Id == id); //Verilen Id'ye sahip duyuru veritabanından bulunur.
            context.Announcements.Remove(anno); //Duyuruyu silinir.
            context.SaveChanges(); //Değişiklikler veritabanına kaydedilir.
            return RedirectToAction("Index"); //Index sayfasına yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int id) //Admin panelindeki seçilen duyuruyu güncellememiz için gereken boş formun açılmasını sağlar.
        {
            var anno = await context.Announcements.FirstOrDefaultAsync(w => w.Id == id); //Verilen Id'ye sahip duyuru veritabanından bulunur. 
            return View(anno); //Güncelleme için View'a gönderir.
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(Announcement us) //Admin panelindeki seçilen duyuruyu güncellememiz için gereken boş formun doldurulduktan sonra kaydedilmesini sağlar.
        {
            if (string.IsNullOrEmpty(us.PhotoUrl)) //Eğer fotoğraf URL'si belirtilmemişse
            {
                us.PhotoUrl = "https://i.hizliresim.com/b5t2vap.jpg"; //buradaki belirttiğim URL atanır.
            }

            var annoupdate = await context.Announcements.FirstOrDefaultAsync(w => w.Id == us.Id); //Verilen Id'ye sahip duyuru veritabanından bulunur. 
            annoupdate.Title = us.Title; //Yeni girilen başlığı alır.
            annoupdate.Body = us.Body; //Yeni girilen içeriği alır.
            annoupdate.PhotoUrl = us.PhotoUrl; //Yeni girilen resmin urlsini alır.

            context.Update(annoupdate); //Güncelleme veritabanına eklenir.
            context.SaveChanges(); //Değişiklikler veritabanına kaydedilir.
            return RedirectToAction("Index"); //Index sayfasına yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public IActionResult ShareToGroup(int id) //Admin panelindeki seçilen duyuruyu göndermemiz için gereken formun açılmasını sağlar.
        {
            var anno = context.Announcements.FirstOrDefault(w => w.Id == id); // Verilen Id'ye sahip duyuru bulunur.
            return View(anno); //Paylaşım için View'a gönderir.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DropdownGroup(string name) //Admin panelindeki seçilen duyuruyu göndermemiz için gereken formun içine görev seçebilmemiz için partial view eklememizi sağlar.
        {
            ViewData["Name"] = name; //ViewBag aracılığıyla View'da kullanılacak olan "Name" değerini atarız.
            List<Role> roles = context.Roles.ToList(); //Tüm rolleri içeren Roles tablosu veritabanından çekilir ve bir Role listesi oluşturulur.
            roles = roles.Where(r => r.Name != "Admin").ToList(); //"Admin" rolü hariç tutularak filtrelenen rollerin yeni bir listesi oluşturulur.
            return PartialView("Partials/_RoleDropdown", roles); //Partial View olan "_RoleDropdown.cshtml" dosyasına, rol listesiyle birlikte yönlendirme yapılır.
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ShareToGroup(FcmRequestModel model, int roleId, SentAnnouncement sentAnno) //Admin panelindeki seçilen duyuruyu göndermemiz için gereken formun doldurulduktan sonra gönderilmesini sağlar.
        {
            try
            {
                string apiUrl = "https://localhost:7051/api/Api/sendNotification"; //Gönderme işlmeminin yapılacağı Apinin urlsi.

                var usersInRole = context.UserRoleRelation.Where(ur => ur.RoleId == roleId).Select(ur => ur.User.DeviceToken).ToList(); //Role bağlı kullanıcıların cihaz token'ları çekilir.Mesela öğrenci seçilmiş ise tüm öğrencilerin cihaz tokenleri alınır.

                if (usersInRole.Any())
                {
                    HttpClient httpClient = new HttpClient(); //HTTP isteği yapmak için HttpClient nesnesi oluşturulur.

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

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData); //Model JSON formatına dönüştürülür.
                        var content = new StringContent(json, Encoding.UTF8, "application/json"); //JSON içeriği ile HTTP isteği oluşturulur.

                        var response = await httpClient.PostAsync(apiUrl, content); //HTTP isteği gönderilir ve cevap alınır.

                        if (!response.IsSuccessStatusCode) //Eğer istek başarısız olursa
                        {
                            ViewBag.ResponseMessage = "Veri gönderme başarısız. HTTP kodu: " + response.StatusCode; //Hata mesajı gösterilir.
                            return RedirectToAction("Index", "SentAnnouncement"); //SentAnnouncement sayfasına yönlendirme yapılır.
                        }
                    }

                    ViewBag.ResponseMessage = "Veri başarıyla gönderildi."; //Veri başarıyla gönderildiyse, başarılı mesajı gösterilir.

                    var sentAnnouncement = new SentAnnouncement //SentAnnouncement tablosuna gönderilen duyuruyu eklemek için SentAnnouncement nesnesi oluşturulur.
                    {
                        AnnouncementId = sentAnno.Id, //SentAnnouncement tablosunda bulunan Id özelliği, sentAnno nesnesinin AnnouncementId özelliği ile doldurulur.
                        RoleId = sentAnno.RoleId, //SentAnnouncement tablosunda bulunan RoleId özelliği, sentAnno nesnesinin RoleId özelliği ile doldurulur.
                    };

                    context.SentAnnouncement.Add(sentAnnouncement); //Oluşturulan SentAnnouncement nesnesi veritabanına eklenir.
                    context.SaveChanges(); //Değişiklikler veritabanına kaydedilir.
                }
            }
            catch (Exception ex) //Hata oluşursa
            {
                ViewBag.ResponseMessage = "Hata oluştu: " + ex.Message; //Hata mesajı gösterilir.
            }

            return RedirectToAction("Index", "SentAnnouncement"); //İşlem tamamlandıktan sonra SentAnnouncement kontrolcüsünün Index metodu çağrılarak Index sayfasına yönlendirme yapılır.
        }
    }
}
