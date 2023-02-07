using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Brand> GetAllBrands();

        IEnumerable<Section> GetAllSection();

        IEnumerable<Product> GetProducts(ProductFilter filter = null);

        Product GetProductById(int id);

        void Update(Product product);

        bool Delete(int id);
    }
}
