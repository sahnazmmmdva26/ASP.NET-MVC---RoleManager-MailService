using Microsoft.AspNetCore.Mvc;
using ProniaSite.DAL;
using ProniaSite.Models;
using ProniaSite.ViewModels;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Brand> brands = new List<Brand>();
            
                brands = _context.Brands.ToList();
            
            return View(brands);
        }
        public IActionResult Delete(int id)
        {
            
                Brand brand = _context.Brands.Find(id);
                if (brand is null) return NotFound();
                _context.Brands.Remove(brand);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            Brand brand = new Brand();
            
                brand = _context.Brands.Find(id);
                if (brand is null) return NotFound();
            
            return View(brand);
        }
        [HttpPost]
        public IActionResult Update(int? id, Brand brand)
        {
            if (id == null) return BadRequest();
            Brand existBrand = new Brand();
            if (brand is null) return NotFound();
            
                existBrand = _context.Brands.Find(id);
                existBrand.Image = brand.Image;
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBrandVM brandVM)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }
            IFormFile file = brandVM.image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz fayl shekil deyil");
                return View();
            }
           
            string fileName = Guid.NewGuid() + (file.FileName.Length > 64 ? file.FileName.Substring(0, 64) : file.FileName);

            using (var stream = new FileStream("C:\\Users\\HP\\Desktop\\c# files\\ProniaSite\\wwwroot\\assets\\images\\website-images" + fileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Brand brand = new Brand() { Image = fileName };
           
                _context.Brands.Add(brand);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
