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
        CreateMap<AddDeviceRequest, Device>();
        CreateMap<Address, AddressInfo>();
        CreateMap<HomeOptions, InfoResponse>()
            .ForMember(m => m.AddressInfo,
                opt => opt.MapFrom(src => src.Address));
    }
}