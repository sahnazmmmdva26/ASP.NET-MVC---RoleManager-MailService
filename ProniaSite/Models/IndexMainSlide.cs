using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaSite.Models
{
    public class IndexMainSlide
    {
        public int Id { get; set; }
        public string Discount { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        [NotMapped]

        public IFormFile image { get; set; }
    }
}
