using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Services.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces;
using WebStore.Domain.Models;
using WebStore.Domain.DTO;
using WebStore.Services.Mapping;

namespace WebStore.Services.InSQL
{
    public class SqlProductData: IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public bool Delete(int id)
        {
            var product = _db.Products
                .Include(product => product.Brand)
                .Include(product => product.Section)
                .FirstOrDefault(p => p.Id == id);

            if (product is null) return false;

           // await using var transaction = await _db.Database.BeginTransactionAsync().ConfigureAwait(false);

            _db.Entry(product).State = EntityState.Deleted;
            //_db.Products.Remove(product);

           
            _db.SaveChanges();
           //await transaction.CommitAsync();
            return true;
        }

        public IEnumerable<BrandDTO> GetBrands() => _db.Brands.Include(b => b.Products).ToDTO();

        public BrandDTO GetBrandById(int id) => _db.Brands
            .Include(b => b.Products)
            .FirstOrDefault(b => b.Id == id)
            .ToDTO();


        public IEnumerable<BrandDTO> GetAllBrands() => _db.Brands.ToDTO().ToList();

        public IEnumerable<SectionDTO> GetAllSection() => _db.Sections.ToDTO().ToList();

        public ProductDTO GetProductById(int id) => _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Section)
            .FirstOrDefault(product => product.Id == id).ToDTO();

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(product => product.Brand)
                .Include(product => product.Section);

            if (filter?.Ids?.Length > 0)
                query = query.Where(s => filter.Ids.Contains(s.Id));
            else
            {
                if (filter?.SectionId is { } section_id)
                    query = query.Where(s => s.SectionId == section_id);

                if (filter?.BrandId is { } brand_id)
                    query = query.Where(b => b.BrandId == brand_id);
            }

            return query.ToDTO();
        }

        public IEnumerable<SectionDTO> GetSections() => _db.Sections.Include(s => s.Products).ToDTO();

        public SectionDTO GetSectionById(int id) => _db.Sections
            .Include(s => s.Products)
            .FirstOrDefault(s => s.Id == id)
            .ToDTO();


        public void Update(ProductDTO product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));
            //if (_db.Products.Contains(product)) return;

            var db_item_find = GetProductById(product.Id);
            var db_item = db_item_find.FromDTO();
            if (db_item is null) return;

            db_item.Id = product.Id;
            db_item.Name = product.Name;
            db_item.BrandId = product.Brand.Id;
            db_item.SectionId = product.Section.Id;
            db_item.ImageUrl = product.ImageUrl;
            db_item.Price = product.Price;

            _db.Products.Update(db_item);
            _db.SaveChanges();
        }
    }
}
