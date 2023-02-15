using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, WebAPI.Products)
        {
        }

        public bool Delete(int id) => Delete($"{Address}/{id}").IsSuccessStatusCode;

        public IEnumerable<BrandDTO> GetAllBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/allbrands");

        public IEnumerable<SectionDTO> GetAllSection() => Get<IEnumerable<SectionDTO>>($"{Address}/allsections");

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/brands");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{Address}/{id}");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null) =>
            Post(Address, filter ?? new ProductFilter())
            .Content
            .ReadAsAsync<IEnumerable<ProductDTO>>()
            .Result;

        public IEnumerable<SectionDTO> GetSections() => Get<IEnumerable<SectionDTO>>($"{Address}/sections");

        public void Update(ProductDTO product) => Put(Address, product);
    }
}
