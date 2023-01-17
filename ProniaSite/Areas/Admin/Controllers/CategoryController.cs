using Microsoft.AspNetCore.Mvc;
using ProniaSite.DAL;
using ProniaSite.Models;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = new List<Category>();
           
                categories=_context.Categories.ToList();
            
            return View(categories);
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
                if (category is null) return NotFound();
                _context.Categories.Remove(category);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if(id== null) return BadRequest();
            Category category = new Category();
           
               category = _context.Categories.Find(id);
                if (category is null) return NotFound();
            
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int? id, Category category)
        {
            if(id==null) return BadRequest();
            Category existCategory = new Category();
            if(category is null) return NotFound();
            
                existCategory= _context.Categories.Find(id);
                existCategory.Name=category.Name;
                existCategory.Count = category.Count;
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            
                _context.Categories.Add(category);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
