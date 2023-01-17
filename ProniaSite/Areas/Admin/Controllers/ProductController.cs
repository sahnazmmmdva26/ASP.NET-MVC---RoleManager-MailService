using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProniaSite.DAL;
using ProniaSite.Models;
using ProniaSite.Utilies;
using ProniaSite.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace ProniaSite.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.CurrentPage= page;
            ViewBag.MaxPageCount = Math.Ceiling((decimal)_context.Products.Count()/5);
            IEnumerable<Product> products= _context.Products.Skip((page - 1) * 5).Take(5).Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(p => p.ProductImages);
            return View(products);
        }
        public IActionResult Create()
        {
            
            ViewBag.Colors = new SelectList(_context.Colors,"Id","Name");
            ViewBag.Sizes = new SelectList(_context.Sizes,"Id","Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {
            var coverimg = cp.CoverImage;
            var hoverimg =cp.HoverImage;
            var otherimg = cp.OtherImages;
            
            if(hoverimg?.CheckType("image/")==false)
            {
                ModelState.AddModelError("Hover", "yuklediyiniz fayl shekil deyil");
            }
            if (hoverimg?.CheckSize(300) == true)
            {
                ModelState.AddModelError("Hover", "yuklediyiniz faylin olcusu 300kb dan az olmalidi");
            }
            if(coverimg?.CheckType("image/")==false)
            {
                ModelState.AddModelError("Cover", "yuklediyiniz fayl shekil deyil");
            }
            if (coverimg?.CheckSize(300) == true)
            {
                ModelState.AddModelError("Cover", "yuklediyiniz faylin olcusu 300kb dan az olmalidi");
            }


                if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
                ViewBag.Sizes = new SelectList(_context.Sizes, "Id", "Name");
                return View();
            }
            var sizes = _context.Sizes.Where(s => cp.SizeIds.Contains(s.Id));
            var colors = _context.Colors.Where(c => cp.ColorIds.Contains(c.Id));
          
            Product newProduct = new Product
            {
                Name = cp.Name,
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                Description = cp.Description,
                Discount = cp.Discount,
                IsDeleted = false,
                SKU = "1"
            };
            List<ProductImage> images = new List<ProductImage>();
            images.Add(new ProductImage
            {
                ImageUrl = coverimg.SaveFile(Path.Combine(_env.WebRootPath, "assets",
                "images", "website-images")),
                IsCover = true,
                Product = newProduct
            });
            images.Add(new ProductImage
            {
                ImageUrl = hoverimg.SaveFile(Path.Combine(_env.WebRootPath, "assets",
               "images", "website-images")),
                IsCover = false,
                Product = newProduct
            });
            foreach (var item in otherimg?? new List<IFormFile> ())
            {
                if (item?.CheckType("image/") == false)
                {
                    ModelState.AddModelError("Hover", "yuklediyiniz fayl shekil deyil");
                }
                if (item?.CheckSize(300) == false)
                {
                    ModelState.AddModelError("Hover", "yuklediyiniz faylin olcusu 300kb dan az olmalidi");
                }
                images.Add(new ProductImage
                {
                    ImageUrl = item.SaveFile(Path.Combine(_env.WebRootPath, "assets",
               "images", "website-images")),
                    IsCover = null,
                    Product = newProduct
                });
            }

            newProduct.ProductImages = images;
            _context.Products.Add(newProduct);
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor { Product=newProduct,ColorId=item.Id});
            }
            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize { Product = newProduct, SizeId = item.Id });
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if(id == null) return BadRequest();
            Product product = _context.Products.Include(p => p.ProductColors).FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            foreach (ProductImage image in product.ProductImages ?? new List<ProductImage>())
            {
                image.ImageUrl.DeleteFile(_env.WebRootPath, "assets/images/website-images");
            }
             
            _context.Products.Remove(product);
            product.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            Product product = _context.Products.Include(p => p.ProductColors).Include(p => p.ProductSizes).FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            UpdateProductVM productVM = new UpdateProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Discount = product.Discount,
                SellPrice = product.SellPrice,
                CostPrice = product.CostPrice,
                ColorIds = product.ProductColors.Select(p=>p.ColorId).ToList(),
                SizeIds = product.ProductSizes.Select(p => p.SizeId).ToList()
            };
          
            ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
            ViewBag.Sizes = new SelectList(_context.Sizes, "Id", "Name");

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Update(int? id, UpdateProductVM updateProduct)
        {
            if (id == null) return NotFound();
            foreach (int colorId in (updateProduct.ColorIds ?? new List<int>()))
            {
                if (!_context.Colors.Any(c => c.Id == colorId))
                {
                    ModelState.AddModelError("ColorIds", "wrong input");
                    break;
                }
            }
            foreach (int sizeId in (updateProduct.SizeIds ?? new List<int>()))
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "wrong input");
                    break;
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                return View();
            }
            var prod = _context.Products.Include(p => p.ProductColors).Include(p => p.ProductSizes).FirstOrDefault(p => p.Id == id);
            if (prod == null) return NotFound();
            foreach (var item in prod.ProductColors)
            {
                if (updateProduct.ColorIds.Contains(item.ColorId))
                {
                    updateProduct.ColorIds.Remove(item.ColorId);
                }
                else
                {
                    _context.ProductColors.Remove(item);
                }
            }
            foreach (var colorId in updateProduct.ColorIds)
            {
                _context.ProductColors.Add(new ProductColor { Product = prod, ColorId = colorId });
            }
            prod.Name = updateProduct.Name;
            prod.Discount = updateProduct.Discount;
            prod.CostPrice = updateProduct.CostPrice;
            prod.SellPrice = updateProduct.SellPrice;
            prod.Description = updateProduct.Description;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UpdateProductImage(int? id)
        {
            if (id == null) return BadRequest();
            var prod = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id);
            if (prod == null) return NotFound();
            UpdateProductImageVM updateProductImage = new UpdateProductImageVM
            {
                ProductImages = prod.ProductImages.Where(pi => pi.IsCover == null)
            };
            return View(updateProductImage);
        }
        public IActionResult DeleteProductImage(int? id)
        {
            if (id == null) return BadRequest();
            var productImage = _context.ProductImages.Find(id);
            if (productImage == null) return NotFound();
            _context.ProductImages.Remove(productImage);
            _context.SaveChanges();
            return Ok();
        }
    }
}

  

   