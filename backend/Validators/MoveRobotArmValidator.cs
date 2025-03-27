namespace RoboticArmSim.Validators;
using FluentValidation;
using RoboticArmSim.Models;

public class MoveRobotArmValidator : AbstractValidator<MovementCommand>
{
    public MoveRobotArmValidator()
    {

        RuleFor(x => x.Joint)
            .InclusiveBetween(0, 5).WithMessage("Joint index must be between 0 and 5.");

        RuleFor(x => x.Angle)
            .GreaterThanOrEqualTo(0).WithMessage("Angle must be be between 0 and 180 degrees.")
            .LessThanOrEqualTo(180).WithMessage("Angle must be be between 0 and 180 degrees.");
    }
}