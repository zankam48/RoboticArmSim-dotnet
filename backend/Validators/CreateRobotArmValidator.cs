namespace RoboticArmSim.Validators;
using FluentValidation;
using RoboticArmSim.DTOs;

public class CreateRobotArmValidator : AbstractValidator<CreateRobotArmDTO>
{
    public CreateRobotArmValidator()
    {
        RuleFor(x => x.JointAngles)
            .NotNull().WithMessage("Joint angles are required.")
            .Must(joints => joints.Count == 6).WithMessage("Invalid number of joints. Expected 6.");
        
        RuleForEach(x => x.JointAngles)
            .InclusiveBetween(0, 180)
            .WithMessage("Each joint angle must be between 0 and 180 degrees.");
    }
}