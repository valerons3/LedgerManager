using FluentValidation;
using LedgerManager.Application.Contracts.Residents;

namespace LedgerManager.Application.Validators;

public class UpdateResidentRequestValidator : AbstractValidator<UpdateResidentRequest>
{
    public UpdateResidentRequestValidator()
    {
        RuleFor(x => x.firstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100);

        RuleFor(x => x.lastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100);

        RuleFor(x => x.middleName)
            .MaximumLength(100).WithMessage("Middle name cannot be longer than 100 characters");

        RuleFor(x => x.birthDate)
            .NotEmpty().WithMessage("Birth date is required")
            .LessThan(DateTime.Today).WithMessage("Birth date must be in the past");

        RuleFor(x => x.accountId)
            .NotEmpty().WithMessage("AccountId is required");
    }
}