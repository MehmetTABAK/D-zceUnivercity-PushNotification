using Newtonsoft.Json;

namespace PushNotificationEntites
{
        public class FcmRequestModel
        {
            [JsonProperty("to")]
            public string To { get; set; }
            [JsonProperty("notification")]
            public NotificationDataModel Notification { get; set; }
        }

        public class NotificationDataModel
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
        }
}