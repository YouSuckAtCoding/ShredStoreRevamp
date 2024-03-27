namespace Contracts.Response.ProductsResponses
{
    public class ProductResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public string Type { get; init; }
        public string Category { get; init; }
        public string Brand { get; init; }
        public string ImageName { get; init; }
    }
}
