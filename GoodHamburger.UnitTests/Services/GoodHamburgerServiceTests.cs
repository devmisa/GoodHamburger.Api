using GoodHamburger.Application.Dtos;
using GoodHamburger.Application.Services;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Infrastructure.Interfaces;
using Moq;
using static GoodHamburger.Application.Dtos.MenuDto;

namespace GoodHamburger.UnitTests.Services
{
    public class GoodHamburgerServiceTests
    {
        private readonly Mock<IGoodHamburgerRepository> _repositoryMock;
        private readonly GoodHamburgerService _service;

        public GoodHamburgerServiceTests()
        {
            _repositoryMock = new Mock<IGoodHamburgerRepository>();
            _service = new GoodHamburgerService(_repositoryMock.Object);
        }

        private static Menu CreateMenu(int id, SandwichType type, bool fries = false, bool soda = false) => new()
        {
            Id = id,
            Sandwich = new Sandwich { Type = type },
            IncludeFrenchFries = fries,
            IncludeSoda = soda
        };

        private static MenuRequest CreateRequest(SandwichType type, bool fries = false, bool soda = false) => new()
        {
            Sandwich = new SandwichDto { Type = type },
            IncludeFrenchFries = fries,
            IncludeSoda = soda
        };

        // GetAllAsync

        [Fact]
        public async Task GetAllAsync_ReturnsAllMenuResponses()
        {
            List<Menu> menus =
            [
                CreateMenu(1, SandwichType.XBurger),
                CreateMenu(2, SandwichType.XEgg, fries: true),
                CreateMenu(3, SandwichType.XBacon, fries: true, soda: true)
            ];
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(menus);

            IEnumerable<MenuResponse> result = await _service.GetAllAsync();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
        {
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

            IEnumerable<MenuResponse> result = await _service.GetAllAsync();

            Assert.Empty(result);
        }

        // Por ID

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsMenuResponse()
        {
            Menu menu = CreateMenu(1, SandwichType.XBurger);
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(menu);

            MenuResponse? result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(SandwichType.XBurger, result.Sandwich.Type);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Menu?)null);

            MenuResponse? result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }

        // Criação

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsCreatedMenuResponse()
        {
            MenuRequest request = CreateRequest(SandwichType.XEgg, fries: true);
            Menu created = CreateMenu(1, SandwichType.XEgg, fries: true);
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Menu>())).ReturnsAsync(created);

            MenuResponse result = await _service.CreateAsync(request);

            Assert.Equal(1, result.Id);
            Assert.Equal(SandwichType.XEgg, result.Sandwich.Type);
            Assert.True(result.IncludeFrenchFries);
            Assert.False(result.IncludeSoda);
        }

        [Fact]
        public async Task CreateAsync_CallsRepositoryOnce()
        {
            MenuRequest request = CreateRequest(SandwichType.XBacon);
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Menu>())).ReturnsAsync(CreateMenu(1, SandwichType.XBacon));

            await _service.CreateAsync(request);

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Menu>()), Times.Once);
        }

        // Atualização

        [Fact]
        public async Task UpdateAsync_ExistingId_ReturnsUpdatedMenuResponse()
        {
            MenuRequest request = CreateRequest(SandwichType.XBacon, fries: true, soda: true);
            Menu updated = CreateMenu(1, SandwichType.XBacon, fries: true, soda: true);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Menu>())).ReturnsAsync(updated);

            MenuResponse? result = await _service.UpdateAsync(1, request);

            Assert.NotNull(result);
            Assert.Equal(SandwichType.XBacon, result.Sandwich.Type);
            Assert.True(result.IncludeFrenchFries);
            Assert.True(result.IncludeSoda);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ReturnsNull()
        {
            MenuRequest request = CreateRequest(SandwichType.XBurger);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Menu>())).ReturnsAsync((Menu?)null);

            MenuResponse? result = await _service.UpdateAsync(99, request);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_SetsCorrectIdOnMenu()
        {
            MenuRequest request = CreateRequest(SandwichType.XEgg);
            _repositoryMock.Setup(r => r.UpdateAsync(It.Is<Menu>(m => m.Id == 5))).ReturnsAsync(CreateMenu(5, SandwichType.XEgg));

            MenuResponse? result = await _service.UpdateAsync(5, request);

            Assert.NotNull(result);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Menu>(m => m.Id == 5)), Times.Once);
        }

        // Delete

        [Fact]
        public async Task DeleteAsync_ExistingId_ReturnsTrue()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            bool result = await _service.DeleteAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

            bool result = await _service.DeleteAsync(99);

            Assert.False(result);
        }

        // Menu

        [Fact]
        public void GetMenu_ReturnsAllFiveItems()
        {
            IEnumerable<object> menu = _service.GetMenu();

            Assert.Equal(5, menu.Count());
        }
    }
}
