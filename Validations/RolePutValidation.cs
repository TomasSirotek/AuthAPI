using AuthAPI.Identity.BindingModels;
using FluentValidation;

namespace AuthAPI.Validations; 

public class RolePutValidation : AbstractValidator<RolePutModel> {
    public RolePutValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id).NotNull().WithMessage("Id cannot be empty");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Must insert name");
    }
}