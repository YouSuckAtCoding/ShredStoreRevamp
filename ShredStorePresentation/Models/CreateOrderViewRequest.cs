namespace ShredStorePresentation.Models
{
    public class CreateOrderViewRequest
    {
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        
        public int CSV { get; set; }
        public DateOnly ExpDate { get; set; }
    }
}
