using System.Text.RegularExpressions;
using AuthAPI.Identity.BindingModels.Authentication;
using FluentValidation;

namespace AuthAPI.Validations; 

public class RegisterValidator :  AbstractValidator<RegisterPostModel> {
    public RegisterValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase)
            .WithMessage("Invalid first name format");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase)
            .WithMessage("Invalid last name format");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .Password();
    }
}