using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Home;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;

namespace HomeApi;

/// <summary>
/// Настройки маппинга всех сущностей приложения
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// В конструкторе настроим соответствие сущностей при маппинге
    /// </summary>
    public MappingProfile()
    {
        CreateMap<AddRoomRequest, Room>();

        CreateMap<AddDeviceRequest, Device>()
            .ForMember(m => m.Location,
                opt => opt.MapFrom(src => src.RoomLocation));

        CreateMap<Address, AddressInfo>();

        CreateMap<HomeOptions, InfoResponse>()
            .ForMember(m => m.AddressInfo,
                opt => opt.MapFrom(src => src.Address));

        CreateMap<Device, DeviceView>();
        CreateMap<Device[], GetDevicesResponse>()
            .ForMember(dest => dest.DeviceAmount,
                opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Devices,
                opt => opt.MapFrom(src => src));

        CreateMap<Room, RoomView>();
        CreateMap<Room[], GetRoomsResponse>()
            .ForMember(dest => dest.RoomAmount,
                opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Rooms,
                opt => opt.MapFrom(src => src));
    }
}