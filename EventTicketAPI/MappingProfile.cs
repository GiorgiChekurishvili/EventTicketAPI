using AutoMapper;
using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;
using EventTicketAPI.Services;

namespace EventTicketAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User,  UserLoginDto>().ReverseMap();
            CreateMap<Event, AddEventDto>().ReverseMap();
            CreateMap<TicketType, AddTicketTypeDto>().ReverseMap();
            CreateMap<TicketSale, BuyTicketDto>().ReverseMap();
            CreateMap<Event, EventReturnDto>().ReverseMap();
            CreateMap<Category, CategoryReturnDto>().ReverseMap();
            CreateMap<TicketSale, MyTicketReturnDto>().ReverseMap();
            CreateMap<UserReturnDto, User>().ReverseMap();
            CreateMap<TicketSale, MyTicketReturnDto>()
            .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.EventName))
            .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.TicketTypeName))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate)).ReverseMap();
            CreateMap<TicketType, TicketTypeReturnDto>().ReverseMap();
            CreateMap<Image, ImageDto>().ReverseMap();

        }
    }
}
