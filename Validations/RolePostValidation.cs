using AuthAPI.Identity.BindingModels;
using FluentValidation;

namespace AuthAPI.Validations; 

public class RolePostValidation : AbstractValidator<RolePostModel> {
    public RolePostValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Must insert name");
    }
}