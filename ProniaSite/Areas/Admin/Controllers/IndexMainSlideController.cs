using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaSite.DAL;
using ProniaSite.Models;
using ProniaSite.ViewModels;
using System.Reflection;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IndexMainSlideController : Controller
    {
        private readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public IndexMainSlideController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IndexMainSlideController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<IndexMainSlide> indexMainSlides = new List<IndexMainSlide>();
            
                indexMainSlides = _context.IndexMainSlides.ToList();
            
            return View(indexMainSlides);
        }
        public IActionResult Delete(int id)
        {
            
                IndexMainSlide indexMainSlide = _context.IndexMainSlides.Find(id);
                if (indexMainSlide is null) return NotFound();
                _context.IndexMainSlides.Remove(indexMainSlide);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            IndexMainSlide indexMainSlide = new IndexMainSlide();
           
                indexMainSlide = _context.IndexMainSlides.Find(id);
                if (indexMainSlide is null) return NotFound();
            
            return View(indexMainSlide);
        }
        [HttpPost]
        public IActionResult Update(int? id, IndexMainSlide indexMainSlide)
        {
            if (id == null) return BadRequest();
            IndexMainSlide indexMainSlide1 = new IndexMainSlide();
            if (indexMainSlide is null) return NotFound();
            
                indexMainSlide1 = _context.IndexMainSlides.Find(id);
                indexMainSlide1.Name = indexMainSlide.Name;
                indexMainSlide1.Image = indexMainSlide.Image;
                indexMainSlide1.Description=indexMainSlide.Description;
                indexMainSlide1.Discount=indexMainSlide.Discount;
                
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateIndexMainSlideVM ındexMainSlideVM)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }
            IFormFile file = ındexMainSlideVM.image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz fayl shekil deyil");
                return View();
            }
            
            string fileName = Guid.NewGuid() + (file.FileName.Length > 64 ? file.FileName.Substring(0, 64) : file.FileName);
            

            using (var stream = new FileStream(Path.Combine(_env.WebRootPath,"assets","images", "website-images") + fileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            IndexMainSlide indexMainSlide = new IndexMainSlide { Description = ındexMainSlideVM.Description, Name = ındexMainSlideVM.Name, Image = fileName,Discount= ındexMainSlideVM.Discount};
           
                _context.IndexMainSlides.Add(indexMainSlide);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
           

        }


 
    }
}
