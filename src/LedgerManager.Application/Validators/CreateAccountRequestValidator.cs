using FluentValidation;
using LedgerManager.Application.Contracts.Accounts;

namespace LedgerManager.Application.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");
        
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be later than start date");
        
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200);
        
        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("Area must be greater than 0");
    }
}