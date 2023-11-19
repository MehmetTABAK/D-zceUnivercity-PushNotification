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
		PushNotificationContext context = new();

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			List<GetAnnouncementDTO> annos = context.SentAnnouncement.Include(w=>w.Announcement).ThenInclude(w => w.Roles).ThenInclude(w => w.Role).ToList().Select(anno =>
			new GetAnnouncementDTO
            {
				Id = anno.Id,
				Title = anno.Announcement.Title,
				Body = anno.Announcement.Body,
				RoleNames = anno.Announcement.Roles != null && anno.Announcement.Roles.Any() ? string.Join(", ", anno.Announcement.Roles.Select(w => w.Role?.Name ?? "").Distinct()) : string.Empty,
			}
			).ToList();

			return View(annos);
		}
    }
}
