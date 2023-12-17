using Newtonsoft.Json;

//Bu katman bildirimleri gönderme işlemi gerçekleşeceği zaman buradaki bilgileri alarak (yani to, title ve body) PushNotificationApi katmanına giderek bu bilgilerin Apiye gönderilmesini sağlar.İşlemlerin devamı PushNotificationApi katmanından devam eder.

namespace PushNotificationEntites
{
    public class FcmRequestModel //FCM (Firebase Cloud Messaging) isteği yapmak için kullanılan model sınıfı oluşturuyoruz.
    {
        [JsonProperty("to")]
        public string To { get; set; } //"To" adında string bir property tanımlanıyoruz.Bu özellik, FCM isteğinin gönderileceği hedefi temsil eder.

        [JsonProperty("notification")]
        public NotificationDataModel Notification { get; set; } //"Notification" adında bir NotificationDataModel property tanımlanıyoruz. Bu özellik, FCM bildirim içeriğini temsil eder.
    }

    public class NotificationDataModel //FCM bildirim içeriğini temsil eden model sınıfı oluşturuyoruz.
    {
        [JsonProperty("title")]
        public string Title { get; set; } //"Title" adında string bir property tanımlanıyoruz.Bu özellik, FCM bildirim başlığını temsil eder.

        [JsonProperty("body")]
        public string Body { get; set; } //"Body" adında string bir property tanımlanıyoruz.Bu özellik, FCM bildirim içeriğini temsil eder.
    }
}