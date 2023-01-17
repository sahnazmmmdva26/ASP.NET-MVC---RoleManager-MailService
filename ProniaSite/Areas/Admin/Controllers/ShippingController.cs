using Microsoft.AspNetCore.Mvc;
using ProniaSite.DAL;
using ProniaSite.Models;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Shipping> shippings = new List<Shipping>();
            
            
                shippings = _context.Shippings.ToList();
            
            return View(shippings);
        }
        public IActionResult Delete(int id)
        {
          
                Shipping shipping = _context.Shippings.Find(id);
                if (shipping is null) return NotFound();
                _context.Shippings.Remove(shipping);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            Shipping shipping = new Shipping();
           
                shipping = _context.Shippings.Find(id);
                if (shipping is null) return NotFound();
            
            return View(shipping);
        }
        [HttpPost]
        public IActionResult Update(int? id, Shipping shipping)
        {
            if (id == null) return BadRequest();
            Shipping existShipping = new Shipping();
            if (shipping is null) return NotFound();
           
                existShipping = _context.Shippings.Find(id);
                existShipping.Name = shipping.Name;
                existShipping.Image = shipping.Image;
                existShipping.Description=shipping.Description;
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Shipping shipping)
        {
            
                _context.Shippings.Add(shipping);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
