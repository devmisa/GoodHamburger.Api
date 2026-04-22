namespace GoodHamburger.Application.Dtos
{
    public record class MenuDto
    {
        public record class MenuRequest
        {
            public required SandwichDto Sandwich { get; set; }
            public bool IncludeFrenchFries { get; set; }
            public bool IncludeSoda { get; set; }
        }

        public record class MenuResponse
        {
            public int Id { get; set; }
            public required SandwichDto Sandwich { get; set; }
            public bool IncludeFrenchFries { get; set; }
            public bool IncludeSoda { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Discount { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
