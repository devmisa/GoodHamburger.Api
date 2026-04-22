namespace GoodHamburger.Domain.Constants
{
    public static class DiscountConstants
    {
        public const decimal SandwichFrenchFriesSodaMultiplier = 0.8m;  // 20% desconto
        public const decimal SandwichFrenchFriesMultiplier = 0.9m;      // 10% desconto
        public const decimal SandwichSodaMultiplier = 0.85m;            // 15% desconto
        public const decimal NoDiscount = 1m;
        public const decimal FullPrice = 1m;
        public const decimal ZeroPrice = 0m;
    }
}
