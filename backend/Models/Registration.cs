namespace RoboticArmSim.Models;
using FluentValidation;

public class Registration
{
    public string? Username {get; set;}
    public string? Password {get; set;}
}

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