using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaSite.ViewModels
{
    public class CreateBannerVM
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }
    }
}
