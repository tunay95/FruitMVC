using FruitMVC.Models;

namespace FruitMVC.Areas.Admin.ViewModel
{
    public class UpdateFruitVM:BaseEntity
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string? ImgUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
