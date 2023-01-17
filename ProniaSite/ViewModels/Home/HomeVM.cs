using ProniaSite.Models;
namespace ProniaSite.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<IndexMainSlide> IndexMainSlides { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Shipping> Shippings { get; set; }
        public IEnumerable<ClientSlide> ClientSlides { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}
