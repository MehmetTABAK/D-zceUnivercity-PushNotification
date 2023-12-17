using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationDbEntities;
using PushNotificationDbEntities.DTOs.Notification;
using PushNotificationDbEntities.DTOs.User;

namespace PushNotificationWeb.Controllers
{
	public class SentAnnouncementController : Controller
	{
		PushNotificationContext context = new(); //PushNotificationContext, veritabanı işlemleri için kullanılacak olup "context" adında bir değişken oluşturulur ve bu değişken, PushNotificationContext sınıfının bir örneğiyle doldurulur.Yani, PushNotificationContext sınıfından bir nesne oluşturularak, veritabanı işlemleri için gerekli bağlam hazırlanmış olur.

        [Authorize]
        [HttpGet]
		public async Task<IActionResult> Index() //Admin panelinde gönderilmiş tüm duyuruları görüntülememizi sağlar.
		{
            List<GetAnnouncementDTO> annos = context.SentAnnouncement //Birden fazla tablodan verilere ihtiyacımız olduğu için bir DTO oluştururuz ve bu DTO içerine gereken propertyleri belirterek erişmemizi sağlarız.
				.Include(w=>w.Announcement) //SentAnnouncement tablosundaki Announcement ilişkisi çekilir.
                .ThenInclude(w => w.Roles) //Announcement tablosundaki Roles ilişkisi çekilir.
                .ThenInclude(w => w.Role) //Roles tablosundaki Role ilişkisi çekilir.
                .ToList() //Veritabanından çekilen veriler liste olarak alınır.
                .Select(anno => new GetAnnouncementDTO // DTO (Data Transfer Object) kullanarak duyurunun belirli alanlarını alır.
                {
					Id = anno.Id, //Duyuru Id'si (GetAnnouncementDTO sınıfında tanımlanmış olan "Id" özelliği) alınır.
                    Title = anno.Announcement.Title, //Duyuru başlığı (GetAnnouncementDTO sınıfında tanımlanmış olan "Title" özelliği) alınır.
                    Body = anno.Announcement.Body, //Duyuru içeriği (GetAnnouncementDTO sınıfında tanımlanmış olan "Body" özelliği) alınır.
                    RoleNames = anno.Announcement.Roles != null && anno.Announcement.Roles.Any() ? string.Join(", ", anno.Announcement.Roles.Select(w => w.Role?.Name ?? "").Distinct()) : string.Empty, //Duyurunun rollerinin isimleri, varsa virgülle ayrılarak birleştirilir.Eğer roller yoksa veya boşsa, string.Empty atanır.
                }
                ).ToList(); //DTO'lar bir listeye dönüştürülür.

            return View(annos); //DTO'lar içeren liste, View'a geçirilir ve Index view'ına yönlendirilir.
        } 
    }
}
