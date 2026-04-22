using static GoodHamburger.Application.Dtos.MenuDto;

namespace GoodHamburger.Application.Interfaces
{
    public interface IGoodHamburgerService
    {
        Task<MenuResponse?> GetByIdAsync(int id);
        Task<MenuResponse> CreateAsync(MenuRequest request);
        Task<MenuResponse?> UpdateAsync(int id, MenuRequest request);
        Task<bool> DeleteAsync(int id);
        IEnumerable<object> GetMenu();
    }
}
