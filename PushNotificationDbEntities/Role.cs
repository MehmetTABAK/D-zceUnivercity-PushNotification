using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class Role : Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public string Name { get; set; } //"Name" adında string bir property tanımlanıyoruz.Role ismini temsil eder.
        public string Code { get; set; } //"Code" adında string bir property tanımlanıyoruz.Role kodunu temsil eder.
        public virtual ICollection<UserRoleRelation> Users { get; set; } //"Users" adında bir ICollection<UserRoleRelation> property tanımlanıyoruz.Bu şekilde, UserRoleRelation sınıfından türetilmiş nesnelerin bir koleksiyonunu temsil ediyoruz.
        public virtual ICollection<SentAnnouncement> Announcements { get; set; } //"Announcemets" adında bir ICollection<SentAnnouncement> property tanımlanıyoruz.Bu şekilde, SentAnnouncement sınıfından türetilmiş nesnelerin bir koleksiyonunu temsil ediyoruz.
    }
}
