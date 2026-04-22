using GoodHamburger.Domain.Constants;

namespace GoodHamburger.Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public required Sandwich Sandwich { get; set; }
        public bool IncludeFrenchFries { get; set; }
        public bool IncludeSoda { get; set; }

        public decimal Subtotal =>
            Sandwich.Price
            + (IncludeFrenchFries ? PriceConstants.FrenchFriesPrice : DiscountConstants.ZeroPrice)
            + (IncludeSoda ? PriceConstants.SodaPrice : DiscountConstants.ZeroPrice);

        public decimal Discount => Subtotal * (DiscountConstants.FullPrice - CalculateDiscount());

        public decimal TotalPrice => Subtotal * CalculateDiscount();

        private decimal CalculateDiscount()
        {
            if (IncludeFrenchFries && IncludeSoda)
            {
                return DiscountConstants.SandwichFrenchFriesSodaMultiplier;
            }
            
            if (IncludeFrenchFries)
            {
                return DiscountConstants.SandwichFrenchFriesMultiplier;
            }
            
            if (IncludeSoda)
            {
                return DiscountConstants.SandwichSodaMultiplier;
            }
            
            return DiscountConstants.NoDiscount;
        }
    }
}