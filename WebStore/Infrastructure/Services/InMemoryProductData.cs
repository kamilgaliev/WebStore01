using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            var query = TestData.Products;

            if (filter?.SectionId is { } section_id)
                query = query.Where(s => s.SectionId == section_id);

            if (filter?.BrandId is { } brand_id)
                query = query.Where(b => b.BrandId == brand_id);

            return query;
        }
    }
}
