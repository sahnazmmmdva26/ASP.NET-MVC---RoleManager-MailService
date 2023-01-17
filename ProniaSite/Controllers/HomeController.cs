using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProniaSite.DAL;
using ProniaSite.Models;
using ProniaSite.ViewModels;
using ProniaSite.ViewModels.Basket;
using ProniaSite.ViewModels.User;
using System.Text.RegularExpressions;


namespace ProniaSite.Controllers
{
    public class HomeController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context,UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager)
        {
            _userManager = usermanager;
            _context = context;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            HomeVM home = new HomeVM
            {
                IndexMainSlides = _context.IndexMainSlides,
                Shippings=_context.Shippings,
                Banners=_context.Banners,
                Brands=_context.Brands,
                ClientSlides=_context.ClientSlides,
                Products= _context.Products.Where(p=>p.IsDeleted==false).Include(p=>p.ProductImages),
            };
            return View(home);
        }
        public IActionResult SingleProduct()
        {
            return View();
        }
        public IActionResult Shop()
        {
            List<Category> Categories = new List<Category>();
           
            
                Categories = _context.Categories.ToList();
            
            List<Color> Colors = new List<Color>();
            
                Colors = _context.Colors.ToList();
            
            ViewBag.Colors=Colors;
            return View(Categories);
        }
        public IActionResult Card()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM,string returUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user==null)
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistance,true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View();
            }
            if(returUrl==null)
            {
                return View(nameof(Index));
            }
            else
            {
                return Redirect(returUrl);
            }
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(registerVM.Name);
            if (user != null)
            {
                ModelState.AddModelError("Username", "Bu istifadechi artiq movcuddur");
                return View();
            }
            user = new AppUser()
            { 
               FirstName= registerVM.Name,
               LastName= registerVM.Surname,
               UserName= registerVM.Username,
               Email= registerVM.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index","Home");
        }
        public IActionResult SetSession(string key,string value)
        {
            HttpContext.Session.SetString(key, value);
            return Content("Ok");
        }
        public IActionResult GetSession(string key)
        {
            string value = HttpContext.Session.GetString(key);
            return Content(value);
        }
        public IActionResult SetCookie (string key,string value)
        {
            HttpContext.Response.Cookies.Append(key, value);
            return Content("ok");
        }
        public IActionResult GetCookie (string key)
        {
            return Content( HttpContext.Request.Cookies[key]);
        }
        public IActionResult AddBasket(int? id)
        {
            List<BasketItemVM> items = new List<BasketItemVM>();
            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }
            BasketItemVM item= items.FirstOrDefault(s=>s.Id==id);
            if (item==null)
            {
                item = new BasketItemVM()
                {
                    Id=(int)id,
                    Count=1
                };
                items.Add(item);
            }
            else
            {
                item.Count++ ;
            }
            string basket=JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", basket, new CookieOptions
            {
                MaxAge= TimeSpan.FromDays(5)
            });
            return RedirectToAction(nameof(Index));
        }


    }
}
