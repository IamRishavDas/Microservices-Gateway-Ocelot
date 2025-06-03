using AutoMapper;
using ProductService.DTOs;
using ProductService.Models;

namespace ProductService.Utilities
{
    public class ProductMappingProfile: Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductRespone>()
                .ForMember(productResponse => productResponse.Category, product => product.MapFrom(p => p.Category.ToString()));
            CreateMap<ProductRequest, ProductRespone>()
                .ForMember(productResponse => productResponse.Category, productRequest => productRequest.MapFrom(p => p.Category.ToString()))
                .ReverseMap();
        }
    }
}
