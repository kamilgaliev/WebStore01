using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlProductData: IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands() => _db.Brands.Include(b => b.Products);
        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (filter?.SectionId is { } section_id)
                query = query.Where(s => s.SectionId == section_id);

            if (filter?.BrandId is { } brand_id)
                query = query.Where(b => b.BrandId == brand_id);

            return query;
        }

        public IEnumerable<Section> GetSections() => _db.Sections.Include(s => s.Products);
    }
}
