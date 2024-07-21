using FluentValidation;
using FluentValidation.Results;

namespace SFSimulator.API.Validation.Validators;

public class NullableValidator<T> : AbstractValidator<T>
{
    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        if (context.InstanceToValidate is null)
        {
            return false;
        }

        return base.PreValidate(context, result);
    }
}