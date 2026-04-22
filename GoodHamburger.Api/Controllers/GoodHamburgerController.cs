using FluentValidation.Results;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Validations;
using Microsoft.AspNetCore.Mvc;
using static GoodHamburger.Application.Dtos.MenuDto;

namespace GoodHamburger.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoodHamburgerController(IGoodHamburgerService goodHamburgerService) : ControllerBase
    {
        [HttpGet("menu")]
        public IActionResult GetMenu()
        {
            return Ok(goodHamburgerService.GetMenu());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            MenuResponse? order = await goodHamburgerService.GetByIdAsync(id);
            return order is null ? NotFound(new { message = $"Pedido {id} não encontrado." }) : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuRequest request)
        {
            GoodHamburgerValidator validator = new();
            ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            }

            MenuResponse created = await goodHamburgerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuRequest request)
        {
            GoodHamburgerValidator validator = new();
            ValidationResult validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            }

            MenuResponse? updated = await goodHamburgerService.UpdateAsync(id, request);
            return updated is null ? NotFound(new { message = $"Pedido {id} não encontrado." }) : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await goodHamburgerService.DeleteAsync(id);
            return !deleted ? NotFound(new { message = $"Pedido {id} não encontrado." }) : NoContent();
        }
    }
}
