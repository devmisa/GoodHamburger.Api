using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Infrastructure.Interfaces
{
    public interface IGoodHamburgerRepository
    {
        Task<Menu?> GetByIdAsync(int id);
        Task<Menu> CreateAsync(Menu menu);
        Task<Menu?> UpdateAsync(Menu menu);
        Task<bool> DeleteAsync(int id);
    }
}
