﻿using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces;
using WebStore.Domain.ViewModels;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData) => _ProductData = ProductData;
        public IViewComponentResult Invoke()
        {
            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId is null).FromDTO();

            var parent_sections_views = parent_sections.
                Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                    ProductsCount = s.Products.Count()
                }).ToList();

            int OrderSortMethod(SectionViewModel a, SectionViewModel b) => Comparer<int>.Default.Compare(a.Order, b.Order);

            foreach (var parent_section in parent_sections_views)
            {
                var child = sections.Where(s => s.ParentId == parent_section.Id).FromDTO();

                foreach (var child_section in child)
                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section,
                        ProductsCount = child_section.Products.Count()
                    });

                parent_section.ChildSections.Sort(OrderSortMethod);
            }

            parent_sections_views.Sort(OrderSortMethod);

            return View(parent_sections_views);
        }
    }
}
