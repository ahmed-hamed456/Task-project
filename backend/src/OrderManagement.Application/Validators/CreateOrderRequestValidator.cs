using FluentValidation;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Valid customer ID is required.");
    }
}
