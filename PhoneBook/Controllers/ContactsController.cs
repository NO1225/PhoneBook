using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data.DataAccess;
using PhoneBook.Data.Models;
using PhoneBook.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly PhoneContext _context;


        private readonly string _imagesFolder;

        public ContactsController(PhoneContext context)
        {
            _context = context;
            _imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _context.Contacts.Select(contact => new ContactViewModel()
            {
                Id = contact.Id,
                FullName = contact.FirstName + " " + contact.LastName,
                PhoneNumber = contact.PhoneNumber,
                ImagePath = "images/" + contact.ImagePath
            }).ToListAsync();

            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1024 * 1024 1MB
                if (model.Image.Length > 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Image size must be under 1MB");
                    return View(model);
                }

                if (!model.Image.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("Image", "Personal photo must be a png or jpg");
                    return View(model);
                }

                var ext = Path.GetExtension(model.Image.FileName);

                var name = Guid.NewGuid();

                var contact = new Contact();
                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.PhoneNumber = model.PhoneNumber;
                contact.ImagePath = name + ext;

                while (await _context.Contacts.AnyAsync(c => c.ImagePath == contact.ImagePath))
                {
                    name = Guid.NewGuid();
                    contact.ImagePath = name + ext;
                }

                var path = Path.Combine(_imagesFolder, contact.ImagePath);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
