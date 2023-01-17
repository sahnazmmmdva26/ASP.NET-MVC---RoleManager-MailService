using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaSite.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Image { get; set; }

        [NotMapped]

        public IFormFile image { get; set; }
    }
}
