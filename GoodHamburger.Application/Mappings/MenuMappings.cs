using GoodHamburger.Application.Dtos;
using GoodHamburger.Domain.Entities;
using static GoodHamburger.Application.Dtos.MenuDto;

namespace GoodHamburger.Application.Mappings
{
    public static class MenuMappings
    {
        public static Menu ToEntity(this MenuRequest request) => new()
        {
            Sandwich = new Sandwich { Type = request.Sandwich.Type },
            IncludeFrenchFries = request.IncludeFrenchFries,
            IncludeSoda = request.IncludeSoda
        };

        public static MenuResponse ToResponse(this Menu menu) => new()
        {
            Id = menu.Id,
            Sandwich = new SandwichDto { Type = menu.Sandwich.Type },
            IncludeFrenchFries = menu.IncludeFrenchFries,
            IncludeSoda = menu.IncludeSoda,
            Subtotal = menu.Subtotal,
            Discount = menu.Discount,
            TotalPrice = menu.TotalPrice
        };
    }
}
