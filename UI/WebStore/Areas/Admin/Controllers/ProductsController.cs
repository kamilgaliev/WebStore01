using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Areas.Admin.ViewModels;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Services.Mapping;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData) => _ProductData = ProductData;
        public IActionResult Index()
        {
            return View(_ProductData.GetProducts().FromDTO());
        }

        public IActionResult Edit(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null) return NotFound();

            var brands = _ProductData.GetAllBrands();
            var sections = _ProductData.GetAllSection();
            var product_item = new AdminProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                BrandId = product.Brand.Id,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                SectionId = product.Section.Id,
                BrandItems = brands.FromDTO(),
                SectionItems = sections.FromDTO(),
            };
            return View(product_item);
        }

        [HttpPost]
        public IActionResult Edit(AdminProductViewModel model)
        {
            if(model is null) throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid) return View(model);

            var product = new Product
            {
                Id=model.Id,
                Name = model.Name,
                BrandId = model.BrandId,
                SectionId = model.SectionId,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
            };

            if (product.Id > 0)
            {
                _ProductData.Update(product.ToDTO());
              
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _ProductData.GetProductById(id).FromDTO();
            if (product is null) return NotFound();
            return View(product.ToView());
        }

        public IActionResult DeleteConfirmed(int id)
        {
            _ProductData.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
