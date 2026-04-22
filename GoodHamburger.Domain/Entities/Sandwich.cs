using GoodHamburger.Domain.Constants;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities
{
    public class Sandwich
    {
        public SandwichType Type { get; set; }

        public decimal Price => Type switch
        {
            SandwichType.XBurger => PriceConstants.XBurgerPrice,
            SandwichType.XEgg => PriceConstants.XEggPrice,
            SandwichType.XBacon => PriceConstants.XBaconPrice,
            _ => 0m
        };
    }
}
