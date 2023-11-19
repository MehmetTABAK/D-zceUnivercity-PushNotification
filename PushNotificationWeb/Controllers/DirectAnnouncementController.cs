using Microsoft.AspNetCore.Mvc;
using PushNotificationDataAccess;
using PushNotificationEntites;
using PushNotificationWeb.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using PushNotificationDbEntities;
using Microsoft.EntityFrameworkCore;

namespace PushNotificationWeb.Controllers
{
    public class DirectAnnouncementController : Controller
    {
        PushNotificationContext context = new();

        [HttpGet]
        public IActionResult SendNotify()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendNotify(FcmRequestModel model, int roleId)
        {
            try
            {
                string apiUrl = "https://localhost:7051/api/Api/sendNotification";

                var usersInRole = context.UserRoleRelation.Where(ur => ur.RoleId == roleId).Select(ur => ur.User.DeviceToken).ToList();

                foreach (var deviceToken in usersInRole)
                {
                    var requestData = new FcmRequestModel
                    {
                        To = deviceToken,
                        Notification = new NotificationDataModel
                        {
                            Title = model.Notification.Title,
                            Body = model.Notification.Body
                        }
                    };

                    HttpClient httpClient = new HttpClient();
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.ResponseMessage = "Veri başarıyla gönderildi.";
                    }
                    else
                    {
                        ViewBag.ResponseMessage = "Veri gönderme başarısız. HTTP kodu: " + response.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResponseMessage = "Hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index","Announcement");
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailModel model)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mtabak229@gmail.com", "dkpk ehhq ijvh lugb"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(model.Sender),
                Subject = model.Subject,
                Body = model.Message,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(model.Recipient);

            try
            {
                smtpClient.Send(mailMessage);
                ViewBag.Message = "E-posta gönderildi.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "E-posta gönderme hatası: " + ex.Message;
            }

            return View();
        }
    }
}
