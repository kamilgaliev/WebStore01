﻿using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Interfaces
{
    public interface IProductData
    {
        IEnumerable<SectionDTO> GetSections();

        IEnumerable<BrandDTO> GetBrands();

        IEnumerable<BrandDTO> GetAllBrands();

        IEnumerable<SectionDTO> GetAllSection();

        IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null);

        ProductDTO GetProductById(int id);

        void Update(ProductDTO product);

        bool Delete(int id);
    }
}
