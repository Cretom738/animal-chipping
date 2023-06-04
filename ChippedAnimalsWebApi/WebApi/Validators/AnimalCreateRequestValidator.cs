using FluentValidation;
using Services.Dtos;

namespace WebApi.Validators
{
    public class AnimalCreateDtoValidator : AbstractValidator<AnimalCreateDto>
    {
        public AnimalCreateDtoValidator()
        {
            RuleForEach(acr => acr.Types)
                .GreaterThan(0);
        }
    }
}
