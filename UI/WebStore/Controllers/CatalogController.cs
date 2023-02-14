using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Interfaces;
using WebStore.Services.Mapping;
using WebStore.Domain.ViewModels;

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
                Products = products.OrderBy(p => p.Order).FromDTO().ToView()
            });
        }

        public IActionResult Details(int id)
        {
            var p = _ProductData.GetProductById(id);
            return View(p.FromDTO().ToView());
        }
    }
}
