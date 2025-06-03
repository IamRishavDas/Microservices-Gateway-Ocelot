using ProductService.Models;

namespace ProductService.DTOs
{
    public record ProductRequest(string Name, decimal Price, Category Category);
    public record ProductRespone(Guid Id, string Name, decimal Price, string Category);
}
