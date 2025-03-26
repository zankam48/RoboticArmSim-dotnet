namespace RoboticArmSim.Validators;
using FluentValidation;
using RoboticArmSim.Models;
public class RegistrationValidator : AbstractValidator<Registration>
{
    public RegistrationValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(5,30).WithMessage("Username must be between 5 and 30 characters");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(8,50).WithMessage("Password must be between 8 and 50 characters");
    }
}