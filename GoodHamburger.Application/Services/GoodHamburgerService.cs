using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Domain.Constants;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Interfaces;
using static GoodHamburger.Application.Dtos.MenuDto;

namespace GoodHamburger.Application.Services
{
    public class GoodHamburgerService(IGoodHamburgerRepository repository) : IGoodHamburgerService
    {
        public async Task<IEnumerable<MenuResponse>> GetAllAsync()
        {
            IEnumerable<Menu> menus = await repository.GetAllAsync();
            return menus.Select(m => m.ToResponse());
        }

        public async Task<MenuResponse?> GetByIdAsync(int id)
        {
            Menu? menu = await repository.GetByIdAsync(id);
            return menu?.ToResponse();
        }

        public async Task<MenuResponse> CreateAsync(MenuRequest request)
        {
            Menu created = await repository.CreateAsync(request.ToEntity());
            return created.ToResponse();
        }

        public async Task<MenuResponse?> UpdateAsync(int id, MenuRequest request)
        {
            Menu menu = request.ToEntity();
            menu.Id = id;
            Menu? updated = await repository.UpdateAsync(menu);
            return updated?.ToResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await repository.DeleteAsync(id);
        }

        public IEnumerable<object> GetMenu()
        {
            return
            [
                new { Item = "X Burger",     Category = "Sanduíche",       Price = PriceConstants.XBurgerPrice },
                new { Item = "X Egg",        Category = "Sanduíche",       Price = PriceConstants.XEggPrice },
                new { Item = "X Bacon",      Category = "Sanduíche",       Price = PriceConstants.XBaconPrice },
                new { Item = "Batata Frita", Category = "Acompanhamento",  Price = PriceConstants.FrenchFriesPrice },
                new { Item = "Refrigerante", Category = "Bebida",          Price = PriceConstants.SodaPrice }
            ];
        }
    }
}
