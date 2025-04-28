namespace HomeApi.Contracts.Models.Rooms
{
    /// <summary>
    /// Запрос для обновления данных о комнате
    /// </summary>
    public class EditRoomRequest
    {
        public int NewArea { get; set; }
        public bool NewGasConnected { get; set; }
        public int NewVoltage { get; set; }
    }
}