using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Data;
using GoodHamburger.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GoodHamburger.Infrastructure.Repositories
{
    public class GoodHamburgerRepository(AppDbContext context, ILogger<GoodHamburgerRepository> logger) : IGoodHamburgerRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            logger.LogInformation("Listando todos os pedidos");
            return await _context.Menus.ToListAsync();
        }

        public async Task<Menu?> GetByIdAsync(int id)
        {
            logger.LogInformation("Buscando pedido {Id}", id);
            return await _context.Menus.FindAsync(id);
        }

        public async Task<Menu> CreateAsync(Menu menu)
        {
            _ = await _context.Menus.AddAsync(menu);
            _ = await _context.SaveChangesAsync();
            logger.LogInformation("Pedido criado com Id {Id}", menu.Id);
            return menu;
        }

        public async Task<Menu?> UpdateAsync(Menu menu)
        {
            Menu? existing = await _context.Menus.FindAsync(menu.Id);
            if (existing is null)
            {
                logger.LogWarning("Tentativa de atualizar pedido inexistente {Id}", menu.Id);
                return null;
            }

            existing.Sandwich = menu.Sandwich;
            existing.IncludeFrenchFries = menu.IncludeFrenchFries;
            existing.IncludeSoda = menu.IncludeSoda;

            _ = await _context.SaveChangesAsync();
            logger.LogInformation("Pedido {Id} atualizado", menu.Id);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Menu? menu = await _context.Menus.FindAsync(id);
            if (menu is null)
            {
                logger.LogWarning("Tentativa de remover pedido inexistente {Id}", id);
                return false;
            }

            _ = _context.Menus.Remove(menu);
            _ = await _context.SaveChangesAsync();
            logger.LogInformation("Pedido {Id} removido", id);
            return true;
        }
    }
}
