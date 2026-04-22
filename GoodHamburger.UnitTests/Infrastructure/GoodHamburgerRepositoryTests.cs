using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Infrastructure.Data;
using GoodHamburger.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace GoodHamburger.UnitTests.Infrastructure
{
    public class GoodHamburgerRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly GoodHamburgerRepository _repository;

        public GoodHamburgerRepositoryTests()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new GoodHamburgerRepository(_context, NullLogger<GoodHamburgerRepository>.Instance);
        }

        public void Dispose() => _context.Dispose();

        private static Menu CreateMenu(SandwichType type, bool fries = false, bool soda = false) => new()
        {
            Sandwich = new Sandwich { Type = type },
            IncludeFrenchFries = fries,
            IncludeSoda = soda
        };

        // CreateAsync

        [Fact]
        public async Task CreateAsync_ValidMenu_PersistsAndReturnsWithId()
        {
            Menu menu = CreateMenu(SandwichType.XBurger);

            Menu result = await _repository.CreateAsync(menu);

            Assert.True(result.Id > 0);
            Assert.Equal(1, await _context.Menus.CountAsync());
        }

        [Fact]
        public async Task CreateAsync_ValidMenu_PersistesDadosCorretamente()
        {
            Menu menu = CreateMenu(SandwichType.XEgg, fries: true, soda: true);

            Menu result = await _repository.CreateAsync(menu);

            Menu? saved = await _context.Menus.FindAsync(result.Id);
            Assert.NotNull(saved);
            Assert.Equal(SandwichType.XEgg, saved.Sandwich.Type);
            Assert.True(saved.IncludeFrenchFries);
            Assert.True(saved.IncludeSoda);
        }

        // GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsMenu()
        {
            Menu menu = await _repository.CreateAsync(CreateMenu(SandwichType.XBacon));

            Menu? result = await _repository.GetByIdAsync(menu.Id);

            Assert.NotNull(result);
            Assert.Equal(SandwichType.XBacon, result.Sandwich.Type);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            Menu? result = await _repository.GetByIdAsync(99);

            Assert.Null(result);
        }

        // UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingMenu_UpdatesAndReturns()
        {
            Menu menu = await _repository.CreateAsync(CreateMenu(SandwichType.XBurger));
            menu.Sandwich = new Sandwich { Type = SandwichType.XBacon };
            menu.IncludeFrenchFries = true;

            Menu? result = await _repository.UpdateAsync(menu);

            Assert.NotNull(result);
            Assert.Equal(SandwichType.XBacon, result.Sandwich.Type);
            Assert.True(result.IncludeFrenchFries);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ReturnsNull()
        {
            Menu menu = CreateMenu(SandwichType.XBurger);
            menu.Id = 99;

            Menu? result = await _repository.UpdateAsync(menu);

            Assert.Null(result);
        }

        // DeleteAsync

        [Fact]
        public async Task DeleteAsync_ExistingId_RemovesAndReturnsTrue()
        {
            Menu menu = await _repository.CreateAsync(CreateMenu(SandwichType.XEgg));

            bool result = await _repository.DeleteAsync(menu.Id);

            Assert.True(result);
            Assert.Equal(0, await _context.Menus.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            bool result = await _repository.DeleteAsync(99);

            Assert.False(result);
        }
    }
}
