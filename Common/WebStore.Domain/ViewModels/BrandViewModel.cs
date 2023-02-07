namespace WebStore.Domain.ViewModels
{
    public record BrandViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int Order { get; init; }

        public int ProductsCount { get; init; }
    }
}
