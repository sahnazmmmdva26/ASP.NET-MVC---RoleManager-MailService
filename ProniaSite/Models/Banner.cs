using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaSite.Models
{
    public class Banner
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [NotMapped]

        public IFormFile Image { get; set; }
    }
}
