using Microsoft.AspNetCore.Mvc;
using ProniaSite.DAL;
using ProniaSite.Models;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClientSlideController : Controller
    {
        private readonly AppDbContext _context;

        public ClientSlideController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<ClientSlide> clientSlides = new List<ClientSlide>();
            
                clientSlides = _context.ClientSlides.ToList();
            
            return View(clientSlides);
        }
        public IActionResult Delete(int id)
        {
            
                ClientSlide clientSlide = _context.ClientSlides.Find(id);
                if (clientSlide is null) return NotFound();
                _context.ClientSlides.Remove(clientSlide);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            ClientSlide clientSlide = new ClientSlide();
            
                clientSlide = _context.ClientSlides.Find(id);
                if (clientSlide is null) return NotFound();
            
            return View(clientSlide);
        }
        [HttpPost]
        public IActionResult Update(int? id, ClientSlide clientSlide)
        {
            if (id == null) return BadRequest();
            ClientSlide clientSlide1 = new ClientSlide();
            if (clientSlide is null) return NotFound();
         
                clientSlide1 = _context.ClientSlides.Find(id);
                clientSlide1.Fullname = clientSlide.Fullname;
                clientSlide1.Image = clientSlide.Image;
                clientSlide1.Comment = clientSlide1.Comment;
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ClientSlide clientSlide)
        {
           
                _context.ClientSlides.Add(clientSlide);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
