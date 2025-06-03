using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Models;
using System.Xml.Linq;

namespace ProductService.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductController(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public static List<Product> products = new List<Product>()
        {
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "RAM",
                Price = 1002.12M,
                Category = Category.HARDWARE
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Windows Drive",
                Price = 10.12M,
                Category = Category.HARDWARE
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Flower Vase",
                Price = 89.12M,
                Category = Category.HOME_APPLIENCE
            },
        };

        [HttpGet("connection-string")]
        public ActionResult<string> GetConnectionString()
        {
            return Ok(_configuration["ConnectionStrings:ProductDb"] ?? throw new Exception("Connection string not found!"));
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_mapper.Map<IList<ProductRespone>>(products));
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById([FromRoute]Guid id)
        {
            var product = products.Find(product => product.Id == id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<ProductRespone>(product));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody]ProductRequest productRequest)
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = productRequest.Name,
                Price = productRequest.Price,
                Category = productRequest.Category
            };

            products.Add(product);
            return GetProducts();
        }

        [HttpDelete]
        public IActionResult DeleteProductById([FromQuery]Guid id)
        {

            var product = products.Find(product => product.Id == id);
            if (product == null) return NotFound($"Product with Id={id} id not found");
            products.Remove(product);
            return Ok($"Product with Id={id} deleted successfully!");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductById([FromRoute] Guid id, [FromBody]ProductRequest productRequest)
        {
            var product = products.Find(product => product.Id == id);
            if(product == null) return NotFound($"Product with Id={id} id not found");
            product.Name = productRequest.Name;
            product.Price = productRequest.Price;
            product.Category = productRequest.Category;
            return Ok(_mapper.Map<ProductRespone>(product));
        }
    }
}
