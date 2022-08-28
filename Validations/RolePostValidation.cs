using FluentValidation;
using ProductAPI.Identity.BindingModels;

namespace ProductAPI.Validations; 

public class RolePostValidation : AbstractValidator<RolePostModel> {
    public RolePostValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Must insert name");
    }
}