using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }
        public IActionResult Index(int? BrandId, int? SectionId)
        {
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId
            };

            var products = _ProductData.GetProducts(filter);
            return View( new CatalogViewModel
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Products = products.OrderBy(p => p.Order)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price
                })
            });
        }
    }
}
