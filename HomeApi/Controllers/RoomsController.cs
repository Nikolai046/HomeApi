using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers;

/// <summary>
/// Контроллер комнат
/// </summary>
[ApiController]
[Route("[controller]")]
public class RoomsController : ControllerBase
{
    private IRoomRepository _repository;
    private IMapper _mapper;

    public RoomsController(IRoomRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Просмотр всех существующих комнат
    /// </summary>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _repository.GetAllRooms();

        var resp = new GetRoomsResponse()
        {
            RoomAmount = rooms.Length,
            Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
        };

        return StatusCode(200, resp);
    }

    /// <summary>
    /// Добавление комнаты
    /// </summary>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
    {
        var existingRoom = await _repository.GetRoomByName(request.Name);
        if (existingRoom == null)
        {
            var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
            await _repository.AddRoom(newRoom);
            return StatusCode(201, $"Комната {request.Name} добавлена!");
        }

        return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
    }

    /// <summary>
    /// Обновление данных комнаты
    /// </summary>
    [HttpPut]
    [Route("{name}")]
    public async Task<IActionResult> Add([FromRoute] string name, [FromBody] EditRoomRequest request)
    {
        var editingRoom = await _repository.GetRoomByName(name);
        if (editingRoom == null)
            return StatusCode(400, $"Ошибка: Комната с именем \"{name}\" не существует.");

        await _repository.UpdateRoom(
            editingRoom,
            new UpdateRoomQuery(request.NewArea, request.NewGasConnected, request.NewVoltage)
        );

        return StatusCode(200, $"Комната с именем \"{name}\" успешно обновлена.");
    }
}