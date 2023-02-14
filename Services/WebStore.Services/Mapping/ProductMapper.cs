using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product product) => product is null ? null
            : new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Brand = product.Brand?.Name
            };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products) => products.Select(ToView);

        public static Product FromView(this ProductViewModel product) => product is null ? null : new Product
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Brand = product.Brand is null ? null : new Brand { Name = product.Name}
        };

        public static ProductDTO ToDTO(this Product Product) => Product is null
            ? null
            : new ProductDTO
            {
                Id = Product.Id,
                Name = Product.Name,
                ImageUrl = Product.ImageUrl,
                Price = Product.Price,
                Order = Product.Order,
                Brand = Product.Brand.ToDTO(),
                Section = Product.Section.ToDTO(),
            };

        public static Product FromDTO(this ProductDTO Product) => Product is null
            ? null
            : new Product
            {
                Id = Product.Id,
                Name = Product.Name,
                ImageUrl = Product.ImageUrl,
                Price = Product.Price,
                Order = Product.Order,
                BrandId = Product.Brand?.Id,
                Brand = Product.Brand.FromDTO(),
                SectionId= Product.Section.Id,
                Section = Product.Section.FromDTO(),
            };

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> Products) => Products.Select(ToDTO);
        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> Products) => Products.Select(FromDTO);

    }
}
