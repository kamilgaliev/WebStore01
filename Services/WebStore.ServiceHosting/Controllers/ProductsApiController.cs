using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

        [HttpGet("allbrands")]
        public IEnumerable<BrandDTO> GetAllBrands() => _ProductData.GetAllBrands();

        [HttpGet("allsections")]
        public IEnumerable<SectionDTO> GetAllSection() => _ProductData.GetAllSection();

        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => _ProductData.GetBrands();

        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id) => _ProductData.GetProductById(id);

        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null) => _ProductData.GetProducts(filter);

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections() => _ProductData.GetSections();

        [HttpPut]
        public void Update(ProductDTO product) => _ProductData.Update(product);

        [HttpDelete("{id}")]
        public bool Delete(int id) => _ProductData.Delete(id);
    }
}
