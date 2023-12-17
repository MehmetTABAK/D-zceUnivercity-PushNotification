using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities.Base
{
    public class Entity //Tüm DbEntities sınıfları için ortak olanları bu sınıfta belirliyoruz. Bunun amacı da kodu tekrar tekrar kullanmamak.Artık Entity sınıfından türetilen her sınıf için içindeki propertyler geçerli olacaktır.
    {
        public int Id { get; set; } //Id değerini tüm DbEntities sınıfları kullanılacağı için bu sınıfta belirtiyoruz. Bu sayede her DbEntities sınıfında Id adında bir property tanımla ihtiyacından kurtulmuş oluyoruz.
    }
}
