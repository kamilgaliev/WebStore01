using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }
        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands() => 
            _ProductData.GetBrands().FromDTO()
            .OrderBy(b => b.Order)
            .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                ProductsCount = b.Products.Count()
            });
    }
}
