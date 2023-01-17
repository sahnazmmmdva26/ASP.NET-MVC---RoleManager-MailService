using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProniaSite.Models;

namespace ProniaSite.DAL
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<ClientSlide> ClientSlides { get; set; }
        public DbSet<IndexMainSlide> IndexMainSlides { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Banner> Banners { get; set; }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<Product>  Products{ get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInformation>  ProductInformations{ get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
