using System.Collections.Generic;

namespace WebStore.Domain.ViewModels
{
    public record CatalogViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; init; }

        public int? SectionId { get; init; }

        public int? BrandId { get; init; }
    }
}
