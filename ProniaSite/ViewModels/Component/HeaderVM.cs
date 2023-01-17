using ProniaSite.ViewModels.Basket;

namespace ProniaSite.ViewModels
{
    public class HeaderVM
    {
        public IDictionary<string,string> Settings { get; set; }

        public BasketVM Basket { get; set; }
    }
}
