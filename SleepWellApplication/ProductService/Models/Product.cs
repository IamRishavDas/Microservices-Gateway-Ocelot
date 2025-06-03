namespace ProductService.Models
{
    public enum Category
    {
        HARDWARE, //0
        HOME_APPLIENCE, //1
    }
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
    }
}
