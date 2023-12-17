using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class Announcement: Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public string Title { get; set; } //"Title" adında string bir property tanımlanıyoruz.Bildirimin başlığını temsil eder.
        public string Body { get; set; } //"Body" adında string bir property tanımlanıyoruz.Bildirimin içeriğini temsil eder.
        public string PhotoUrl { get; set; } //"PhotoUrl" adında string bir property tanımlanıyoruz.Bildirime ait bir fotoğrafın URL'sini temsil eder.
        public virtual ICollection<SentAnnouncement> Roles { get; set; } //"Roles" adında bir ICollection<SentAnnouncement> property tanımlanıyoruz.Bu şekilde, SentAnnouncement sınıfından türetilmiş nesnelerin bir koleksiyonunu temsil ediyoruz.
    }
}
