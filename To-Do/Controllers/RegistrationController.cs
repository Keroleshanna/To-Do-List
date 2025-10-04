using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using To_Do.Data;
using To_Do.Models;

namespace To_Do.Controllers
{
    public class RegistrationController : Controller
    {
        ApplicationDbContext _context = new();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == pass);

            if (user != null) // يعني فيه user صحيح
            {
                // لو عنده ToDoList قديمة
                var ownList = _context.OwnToDoLists.FirstOrDefault(U=> U.UserId == user.Id);

                if (ownList != null)
                {
                    return RedirectToAction(
                        controllerName: "OwnList",
                        actionName: "Items",
                        routeValues: new { id = user.Id } // 👈 هنا بعت الـ User.Id
                    );
                }
                else
                {
                    return RedirectToAction(
                        controllerName: "OwnList",
                        actionName: "Create",
                        routeValues: new { id = user.Id } // 👈 نفس الحكاية هنا
                    );
                }
            }

            return View();
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string name,string email, string pass)
        {
            var checkEmail = _context.Users.FirstOrDefault(E => E.Email == email);
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass) && checkEmail == null)
            {
                User user = new() { Name = name, Email = email, Password = pass };
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "This email already has a to-do list.";
                return View();
            }

            return View("Register");
        }







    }
}
