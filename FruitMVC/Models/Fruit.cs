using System.ComponentModel.DataAnnotations.Schema;

namespace FruitMVC.Models
{
    public class Fruit:BaseEntity
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile Image{ get; set; }
    }
}
