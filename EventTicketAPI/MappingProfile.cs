﻿using AutoMapper;
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
            CreateMap<TicketType, TicketTypeReturnDto>().ReverseMap();
            CreateMap<TicketSale, MyTicketReturnDto>().ReverseMap();
            
        }
    }
}
