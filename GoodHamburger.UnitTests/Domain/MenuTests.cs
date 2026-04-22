using GoodHamburger.Domain.Constants;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.UnitTests.Domain
{
    public class MenuTests
    {
        private static Menu CreateMenu(SandwichType type, bool fries, bool soda) => new()
        {
            Sandwich = new Sandwich { Type = type },
            IncludeFrenchFries = fries,
            IncludeSoda = soda
        };

        // Subtotal

        [Fact]
        public void Subtotal_XBurgerOnly_ReturnsXBurgerPrice()
        {
            Menu menu = CreateMenu(SandwichType.XBurger, false, false);
            Assert.Equal(PriceConstants.XBurgerPrice, menu.Subtotal);
        }

        [Fact]
        public void Subtotal_XEggWithFries_ReturnsSumOfPrices()
        {
            Menu menu = CreateMenu(SandwichType.XEgg, true, false);
            decimal expected = PriceConstants.XEggPrice + PriceConstants.FrenchFriesPrice;
            Assert.Equal(expected, menu.Subtotal);
        }

        [Fact]
        public void Subtotal_XBaconWithFriesAndSoda_ReturnsSumOfAllPrices()
        {
            Menu menu = CreateMenu(SandwichType.XBacon, true, true);
            decimal expected = PriceConstants.XBaconPrice + PriceConstants.FrenchFriesPrice + PriceConstants.SodaPrice;
            Assert.Equal(expected, menu.Subtotal);
        }

        // Sem desconto

        [Fact]
        public void TotalPrice_SandwichOnly_NoDiscount()
        {
            Menu menu = CreateMenu(SandwichType.XBurger, false, false);
            Assert.Equal(PriceConstants.XBurgerPrice, menu.TotalPrice);
        }

        // 10% desconto (sandwich + fritas)

        [Fact]
        public void TotalPrice_SandwichAndFries_Applies10PercentDiscount()
        {
            Menu menu = CreateMenu(SandwichType.XBurger, true, false);
            decimal subtotal = PriceConstants.XBurgerPrice + PriceConstants.FrenchFriesPrice;
            decimal expected = subtotal * DiscountConstants.SandwichFrenchFriesMultiplier;
            Assert.Equal(expected, menu.TotalPrice);
        }

        // 15% desconto (sandwich + refrigerante)

        [Fact]
        public void TotalPrice_SandwichAndSoda_Applies15PercentDiscount()
        {
            Menu menu = CreateMenu(SandwichType.XEgg, false, true);
            decimal subtotal = PriceConstants.XEggPrice + PriceConstants.SodaPrice;
            decimal expected = subtotal * DiscountConstants.SandwichSodaMultiplier;
            Assert.Equal(expected, menu.TotalPrice);
        }

        // 20% desconto (sandwich + fritas + refrigerante)

        [Fact]
        public void TotalPrice_SandwichFriesAndSoda_Applies20PercentDiscount()
        {
            Menu menu = CreateMenu(SandwichType.XBacon, true, true);
            decimal subtotal = PriceConstants.XBaconPrice + PriceConstants.FrenchFriesPrice + PriceConstants.SodaPrice;
            decimal expected = subtotal * DiscountConstants.SandwichFrenchFriesSodaMultiplier;
            Assert.Equal(expected, menu.TotalPrice);
        }

        // Desconto 

        [Fact]
        public void Discount_SandwichOnly_IsZero()
        {
            Menu menu = CreateMenu(SandwichType.XBurger, false, false);
            Assert.Equal(0m, menu.Discount);
        }

        [Fact]
        public void Discount_SandwichAndFries_Is10PercentOfSubtotal()
        {
            Menu menu = CreateMenu(SandwichType.XBurger, true, false);
            decimal subtotal = PriceConstants.XBurgerPrice + PriceConstants.FrenchFriesPrice;
            decimal expected = subtotal * (DiscountConstants.FullPrice - DiscountConstants.SandwichFrenchFriesMultiplier);
            Assert.Equal(expected, menu.Discount);
        }

        [Fact]
        public void Discount_SandwichFriesAndSoda_Is20PercentOfSubtotal()
        {
            Menu menu = CreateMenu(SandwichType.XBacon, true, true);
            decimal subtotal = PriceConstants.XBaconPrice + PriceConstants.FrenchFriesPrice + PriceConstants.SodaPrice;
            decimal expected = subtotal * (DiscountConstants.FullPrice - DiscountConstants.SandwichFrenchFriesSodaMultiplier);
            Assert.Equal(expected, menu.Discount);
        }

        // Preço do sanduíche

        [Fact]
        public void Sandwich_XBurger_HasCorrectPrice()
        {
            Sandwich sandwich = new() { Type = SandwichType.XBurger };
            Assert.Equal(PriceConstants.XBurgerPrice, sandwich.Price);
        }

        [Fact]
        public void Sandwich_XEgg_HasCorrectPrice()
        {
            Sandwich sandwich = new() { Type = SandwichType.XEgg };
            Assert.Equal(PriceConstants.XEggPrice, sandwich.Price);
        }

        [Fact]
        public void Sandwich_XBacon_HasCorrectPrice()
        {
            Sandwich sandwich = new() { Type = SandwichType.XBacon };
            Assert.Equal(PriceConstants.XBaconPrice, sandwich.Price);
        }
    }
}
