using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaSite.ViewModels
{
    public class CreateIndexMainSlideVM
    {
      
        public string Discount { get; set; }
       
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile image { get; set; }
    }
}
