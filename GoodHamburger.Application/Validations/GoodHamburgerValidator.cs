using FluentValidation;
using static GoodHamburger.Application.Dtos.MenuDto;

namespace GoodHamburger.Application.Validations
{
    public class GoodHamburgerValidator : AbstractValidator<MenuRequest>
    {
        public GoodHamburgerValidator()
        {
            _ = RuleFor(x => x.Sandwich)
                .NotNull()
                .WithMessage("O sanduíche é obrigatório.");

            When(x => x.Sandwich is not null, () =>
            {
                RuleFor(x => x.Sandwich.Type)
                    .IsInEnum()
                    .Must(t => t != 0)
                    .WithMessage("Tipo de sanduíche inválido. Use XBurger para 1, XEgg para 2 ou XBacon para 3.");
            });
        }
    }
}
