using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do.Data;
using To_Do.Models;

namespace To_Do.Controllers
{
    public class AllItemsController : Controller
    {
        ApplicationDbContext _context = new();

        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.ownList = _context.OwnToDoLists.Find(id);
            return View();
        }

        [HttpPost]
        public IActionResult Create(AllToDoItem model, IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Path.GetFileName(uploadedFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }

                model.File = "/uploads/" + fileName;


            }
            AllToDoItem newModel = new()
            {
                Title = model.Title,
                Description = model.Description,
                Deedline = model.Deedline,
                File = model.File,
                OwnToDoListId = model.Id
            };

            _context.AllToDoItems.Add(newModel);
            _context.SaveChanges();
            var user = _context.OwnToDoLists.Include(U => U.User).Where(U => U.User.Id == U.UserId).FirstOrDefault();
            return RedirectToAction(
                controllerName: "OwnList",
                actionName: "Items",
                routeValues: new { id = user?.UserId }
            );
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var item = _context.AllToDoItems.Find(id);
            return View(item);
        }

        [HttpPost]
        public IActionResult Update(AllToDoItem itemToDo, IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Path.GetFileName(uploadedFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }

                itemToDo.File = "/uploads/" + fileName;


            }
            _context.AllToDoItems.Update(itemToDo);
            _context.SaveChanges();
            var itemWithUser = _context.AllToDoItems
                .Include(i => i.OwnToDoList)
                    .ThenInclude(l => l.User)
                .FirstOrDefault(i => i.Id == itemToDo.Id);
            var user = itemWithUser?.OwnToDoList?.User;

            return RedirectToAction("Items", "ownList", new { id = user?.Id });

        }


        public IActionResult Delete(int id)
        {
            var item = _context.AllToDoItems.Find(id);

            var itemWithUser = _context.AllToDoItems
                .Include(i => i.OwnToDoList)
                    .ThenInclude(l => l.User)
                .FirstOrDefault(i => i.Id == id);
            var user = itemWithUser?.OwnToDoList?.User;

            if (item is not null)
            {
                _context.AllToDoItems.Remove(item);
                _context.SaveChanges();
            }



            return RedirectToAction("Items", "ownList", new { id = user?.Id });
        }

    }
}
