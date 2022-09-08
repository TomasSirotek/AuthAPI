using System.Text.RegularExpressions;
using AuthAPI.Identity.BindingModels;
using FluentValidation;

namespace AuthAPI.Validations; 

public class PostUserValidation : AbstractValidator<UserPostModel> {
    
    public PostUserValidation()
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
        
        RuleForEach(x => x.Roles)
            .Roles();
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password();

        RuleFor(x => x.IsActivated)
            .NotNull()
            .WithMessage("Invalid is active state");

    }
}

