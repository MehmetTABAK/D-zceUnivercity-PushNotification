using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities.DTOs.Notification
{
    public class GetAnnouncementDTO //Burada gerçekleştirmiş olduğumuz olay iki farklı DbEntities sınıfındaki verileri tek sınıfta kullanmak.Yani oluşturacağımız sayfada bizim Announcement sınıfındaki propertylerin yanında Role sınıfındaki RoleName propertysine ihtiyacımız olduğu için bu sınıfı oluşturuyoruz.Herhangi bir view içinde iki farklı model kullanamadığımız için bu yöntemi kullanarak tek modelde iki farklı tablodaki ihtiyacımız olan verilere ulaşabiliyoruz.
    {
		public int Id { get; set; } //Announcement sınıfından
		public string Title { get; set; } //Announcement sınıfından
        public string Body { get; set; } //Announcement sınıfından
        public string RoleNames { get; set; } //Role sınıfından
    }
}
