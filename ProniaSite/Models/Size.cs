namespace ProniaSite.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductSize>? productSizes { get; set; }
    }
}
