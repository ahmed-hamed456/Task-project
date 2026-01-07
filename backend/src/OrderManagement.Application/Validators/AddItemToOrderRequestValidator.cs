using FluentValidation;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Validators;

public class AddItemToOrderRequestValidator : AbstractValidator<AddItemToOrderRequest>
{
    public AddItemToOrderRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Valid product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
