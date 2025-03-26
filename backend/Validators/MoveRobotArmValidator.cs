namespace RoboticArmSim.Validators;
using FluentValidation;
using RoboticArmSim.Models;

public class MoveRobotArmValidator : AbstractValidator<MovementCommand>
{
    public MoveRobotArmValidator()
    {
        RuleFor(x => x.ArmId)
            .NotNull().WithMessage("Arm ID is required.")
            .GreaterThan(0).WithMessage("Arm ID must be greater than 0.");

        RuleFor(x => x.Joint)
            .InclusiveBetween(0, 5).WithMessage("Joint index must be between 0 and 5.");

        RuleFor(x => x.Angle)
            .GreaterThanOrEqualTo(0).WithMessage("Angle must be 0 or more.")
            .LessThanOrEqualTo(180).WithMessage("Angle must be 180 or less.");
    }
}