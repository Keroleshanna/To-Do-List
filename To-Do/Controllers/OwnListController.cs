using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do.Data;
using To_Do.Models;

namespace To_Do.Controllers
{
    public class OwnListController : Controller
    {

        ApplicationDbContext _context = new();

        [HttpGet]
        public IActionResult Create(int id)
        {
            var newUser = _context.Users.Find(id);
            return View(newUser);
        }

        [HttpPost]
        public IActionResult Create(int id, string name, string description, DateTime date)
        {

            OwnToDoList ownToDoList = new()
            {
                Name = name,
                Description = description,
                CreationDate = date,
                UserId = id
            };
            if (ownToDoList.Name is null || ownToDoList.Description is null)
            {
                ViewBag.Error = "You shud full Name and Description!!!";
                var newUser = _context.Users.Find(id);
                return View(newUser);
            }
            else
            {
                _context.OwnToDoLists.Add(ownToDoList);
                _context.SaveChanges();
                return RedirectToAction("Items", new { id });
            }

        }


        public IActionResult Items(int id)
        {
            ViewBag.herName = _context.Users.Find(id);
            ViewBag.ListName = _context.OwnToDoLists.Where(U => U.UserId == id).FirstOrDefault();
            var myList = _context.AllToDoItems.Include(U => U.OwnToDoList).Where(L=>L.OwnToDoList.UserId == id).ToList();
            return View(myList);
        }



    }
}
