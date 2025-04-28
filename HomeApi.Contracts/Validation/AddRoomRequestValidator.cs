using FluentValidation;
using global::System.Linq;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validation
{
    /// <summary>
    /// Класс-валидатор запросов на добавление новой комнаты
    /// </summary>
    public class AddRoomRequestValidator : AbstractValidator<AddRoomRequest>
    {
        public AddRoomRequestValidator()
        {
            RuleFor(x => x.Area).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().Must(BeSupported)
                .WithMessage($"Please choose one of the following locations: {string.Join(", ", Values.ValidRooms)}"); ;
            RuleFor(x => x.Voltage).NotEmpty();
            RuleFor(x => x.GasConnected)
                .NotNull().WithMessage("GasConnected не может быть null.")
                .Must(x => x == true || x == false).WithMessage("GasConnected должен быть true или false.");
        }

        private bool BeSupported(string location)
        {
            return Values.ValidRooms.Any(e => e == location);
        }
    }
}