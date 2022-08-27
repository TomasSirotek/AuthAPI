using System.Data;
using System.Text.RegularExpressions;
using FluentValidation;
using ProductAPI.Identity.BindingModels;

namespace ProductAPI.Validations; 

public class PutUserValidation : AbstractValidator<UserPutModel> {
    public PutUserValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id)
            .NotEmpty();
        
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

        RuleFor(x => x.IsActivated)
            .NotNull()
            .WithMessage("Invalid is active state");

    }
}