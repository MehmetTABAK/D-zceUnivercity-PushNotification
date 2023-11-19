using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using PushNotificationWeb.Models;
using PushNotificationDataAccess;
using PushNotificationDbEntities;
using Microsoft.EntityFrameworkCore;
using PushNotificationDbEntities.DTOs.User;

namespace PushNotificationWeb.Controllers
{
    public class UserController : Controller
    {
        PushNotificationContext context = new();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<GetUserDTO> users = context.Users.Include(w => w.Roles).ThenInclude(w => w.Role).ToList().Select(user =>
            new GetUserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DeviceToken = user.DeviceToken,
                RoleNames = user.Roles != null && user.Roles.Any() ? string.Join(", ", user.Roles.Select(w => w.Role?.Name ?? "")) : string.Empty,
            }
            ).ToList();

            return View(users);

            //List<User> users = context.Users.ToList();

            //List<GetUserDTO> userDTOs = new List<GetUserDTO>();

            //foreach (var user in users)
            //{
            //    List<string> roleNames = context.UserRoleRelation
            //        .Where(v => v.UserId == user.Id)
            //        .Select(ur => ur.Role.Name)
            //        .ToList();

            //    string roleNamesString = string.Join(", ", roleNames);

            //    GetUserDTO userDTO = new GetUserDTO
            //    {
            //        Id = user.Id,
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        Email = user.Email,
            //        PhoneNumber = user.PhoneNumber,
            //        DeviceToken = user.DeviceToken,
            //        RoleNames = roleNamesString
            //    };
            //    userDTOs.Add(userDTO);
            //}

            //return View(userDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(w => w.Id == id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDTO us)
        {
            var userupdate = await context.Users.FirstOrDefaultAsync(w => w.Id == us.Id);
            userupdate.FirstName = us.FirstName;
            userupdate.LastName = us.LastName;
            userupdate.Email = us.Email;
            userupdate.PhoneNumber = us.PhoneNumber;
            userupdate.DeviceToken = us.DeviceToken;

            var selectedRole = context.UserRoleRelation.FirstOrDefault(w => w.UserId == userupdate.Id)!;
            selectedRole.RoleId= us.RoleId;
            context.Update(selectedRole);

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DropdownRole(string name)
        {
            ViewData["Name"] = name;
            List<Role> roles = context.Roles.ToList();
            return PartialView("Partials/_RoleDropdown", roles);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(w => w.Id == id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DropdownUsers(string name)
        {
            ViewData["Name"] = name;
            List<Role> roles = context.Roles.ToList();
            roles = roles.Where(r => r.Name != "Admin").ToList();
            return PartialView("Partials/_Dropdown", roles);
        }


        [HttpGet]
        public async Task<IActionResult> DropdownEmail(string name)
        {
            ViewData["Name"] = name;
            List<User> users = context.Users.ToList();
            return PartialView("Partials/_EmailDropdown", users);
        }
    }
}
