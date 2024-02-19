namespace Contracts.Response.CartItemResponses
{
    public class CartItemResponse
    {
        public int CartId { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }
        public int ProductId { get; init; }
    }
}