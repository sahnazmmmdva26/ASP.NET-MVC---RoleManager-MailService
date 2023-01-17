using Microsoft.AspNetCore.Mvc;
using ProniaSite.DAL;
using ProniaSite.Models;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Color> colors = new List<Color>();
            
                colors = _context.Colors.ToList();
            
            return View(colors);
        }
        public IActionResult Delete(int id)
        {
            
                Color color = _context.Colors.Find(id);
                if (color is null) return NotFound();
                _context.Colors.Remove(color);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            Color color = new Color();
           
                color = _context.Colors.Find(id);
                if (color is null) return NotFound();
            
            return View(color);
        }
        [HttpPost]
        public IActionResult Update(int? id, Color color)
        {
            if (id == null) return BadRequest();
            Color color1 = new Color();
            if (color is null) return NotFound();
            
                color1 = _context.Colors.Find(id);
                color1.Name = color.Name;
                color1.Count = color.Count;
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Color color)
        {
                _context.Colors.Add(color);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
