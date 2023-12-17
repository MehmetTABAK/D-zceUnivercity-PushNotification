namespace PushNotificationWeb.Models
{
    public class EmailModel
    {
        public string Sender { get; set; } //"Sender" adında string bir property tanımlanıyoruz.Email göndereni temsil eder.
        public string Recipient { get; set; } //"Recipient" adında string bir property tanımlanıyoruz.Email alıcısını temsil eder.
        public string Subject { get; set; } //"Subject" adında string bir property tanımlanıyoruz.Email konusunu temsil eder.
        public string Message { get; set; } //"Message" adında string bir property tanımlanıyoruz.Email içeriğini temsil eder.
    }
}
