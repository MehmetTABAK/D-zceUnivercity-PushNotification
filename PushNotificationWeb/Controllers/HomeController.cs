using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationEntites;
using PushNotificationWeb.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using PushNotificationDbEntities;

namespace PushNotificationWeb.Controllers
{
    public class HomeController : Controller
    {
        PushNotificationContext context = new(); //PushNotificationContext, veritabanı işlemleri için kullanılacak olup "context" adında bir değişken oluşturulur ve bu değişken, PushNotificationContext sınıfının bir örneğiyle doldurulur.Yani, PushNotificationContext sınıfından bir nesne oluşturularak, veritabanı işlemleri için gerekli bağlam hazırlanmış olur.
        public IActionResult Index() //Kullanıcı panelindeki anasayfamızı görüntülememiz sağlar.
        {
            List<Announcement> announcements = context.Announcements.ToList(); //Veritabanından tüm duyurular çekilir.
            return View(announcements); //Duyurular bir liste olarak View'a geçirilir ve Index view'ına yönlendirilir.
        }

        public IActionResult About() //Kullanıcı panelindeki hakkımızda sayfamızı görüntülememiz sağlar.
        {
            return View(); //Hakkında sayfasını gösteren bir sayfa için kullanılır.
        }

        public IActionResult Contact() //Kullanıcı panelindeki iletişim sayfamızı görüntülememiz sağlar.
        {
            return View(); //İletişim sayfasını gösteren bir sayfa için kullanılır.
        }

        public IActionResult Announcement() //Kullanıcı panelindeki duyurular sayfamızı görüntülememiz sağlar.
        {
            List<Announcement> announcements = context.Announcements.ToList(); //Veritabanından tüm duyurular çekilir.
            return View(announcements); //Duyurular bir liste olarak View'a geçirilir ve Announcement view'ına yönlendirilir.
        } 

        public IActionResult AnnouncementDetails(int id) //Kullanıcı panelindeki duyurulara tıklayınca detaylarını görüntülememizi sağlar.
        {
            var anno = context.Announcements.Where(w => w.Id == id).ToList(); //Belirli bir duyuru Id'sine sahip olan duyuru çekilir.
            return View(anno); //Duyuru bir liste olarak View'a geçirilir ve AnnouncementDetails view'ına yönlendirilir.
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() //Hata sayfasını gösteren bir sayfa için kullanılır.
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); //Sayfanın önbelleğini devre dışı bırakır ve hata sayfasına bir model gönderilir.
        }
    }
}