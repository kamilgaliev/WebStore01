using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Areas.Admin.ViewModels
{
    public class AdminProductViewModel
    {
        public IEnumerable<Brand> BrandItems { get; set; }

        public IEnumerable<Section> SectionItems { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; init; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина названия должна быть от 2 до 20 символов")]
        public string Name { get; set; }

        [Display(Name = "Картинка")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        public string ImageUrl { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        public decimal Price { get; set; }

        public int BrandId { get; set; }

        //public string BrandName => BrandItems.FirstOrDefault(b => b.Id == BrandId).Name;

        public int SectionId { get; set; }

        //public string SectionName => SectionItems.FirstOrDefault(b => b.Id == SectionId).Name;
    }
}
