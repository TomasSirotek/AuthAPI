using FluentValidation;
using ProductAPI.Domain.Enum;

namespace ProductAPI.Validations; 

public static class CustomValidators {
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 14)
    {
        var options = ruleBuilder
            .NotEmpty().WithMessage("Cannot be empty")
            .MinimumLength(minimumLength).WithMessage("Min length must be 14 characters")
            .Matches("[A-Z]").WithMessage("Must contain uppercase letters")
            .Matches("[a-z]").WithMessage("Must contain lowercase letters")
            .Matches("[0-9]").WithMessage("Must contain digits 0-1")
            .Matches("[^a-zA-Z0-9]").WithMessage("Must contain special characters");
        return options;
    }
    public static IRuleBuilder<T, string> Roles<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var options = ruleBuilder
            .NotEmpty().WithMessage("Cannot be empty")
            .IsEnumName(typeof(AccessRole)).WithMessage("Must be one of the accepted roles");
            
        return options;
    }
}