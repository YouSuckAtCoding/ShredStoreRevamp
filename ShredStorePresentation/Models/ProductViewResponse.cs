﻿namespace ShredStorePresentation.Models
{
    public class ProductViewResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string ImageName { get; set; }
    }
}
