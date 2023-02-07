using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.Infrastructure.Services.InSQL
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

        public IEnumerable<Brand> GetBrands() => _db.Brands.Include(b => b.Products);

        public IEnumerable<Brand> GetAllBrands() => _db.Brands.ToList();

        public IEnumerable<Section> GetAllSection() => _db.Sections.ToList();

        public Product GetProductById(int id) => _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Section)
            .FirstOrDefault(product => product.Id == id);

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
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

            return query;
        }

        public IEnumerable<Section> GetSections() => _db.Sections.Include(s => s.Products);

        public void Update(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));
            //if (_db.Products.Contains(product)) return;

            var db_item = GetProductById(product.Id);
            if (db_item is null) return;

            db_item.Id = product.Id;
            db_item.Name = product.Name;
            db_item.BrandId = product.BrandId;
            db_item.SectionId = product.SectionId;
            db_item.ImageUrl = product.ImageUrl;
            db_item.Price = product.Price;

            _db.Products.Update(db_item);
            _db.SaveChanges();
        }
    }
}
