using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    //Cross Table
    //Cross Table iki veya daha fazla tablonun birbirleriyle ilişkilendirildiği ve genellikle çoktan çok ilişkileri temsil ettiği bir tür veritabanı tablosudur.Biz burada Roles ve Announcements tablolarına çoktan çok ilişki kurduruyoruz.
    public class SentAnnouncement: Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public int AnnouncementId { get; set; } //"AnnouncementId" adında int bir property tanımlanıyoruz.Gönderilen bildirimin ilişkilendiği bildirimin kimliğini temsil eder.
        public int RoleId { get; set; } //"RoleId" adında int bir property tanımlanıyoruz.Gönderilen bildirimin ilişkilendiği rolün kimliğini temsil eder.
        public virtual Role Role{ get; set; } //"Role" adında bir Role property tanımlanıyoruz.Bu özellik, gönderilen bildirimin ilişkilendiği rolü temsil eder.
        public virtual Announcement Announcement { get; set; } //"Announcement" adında bir Announcement property tanımlanıyoruz.Bu özellik, gönderilen bildirimin ilişkilendiği bildirimi temsil eder.
    }
}
