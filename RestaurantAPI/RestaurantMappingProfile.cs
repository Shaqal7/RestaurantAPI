﻿namespace RestaurantAPI
{
	using AutoMapper;
	using RestaurantAPI.Entities;
	using RestaurantAPI.Models;

	public class RestaurantMappingProfile : Profile
	{
		public RestaurantMappingProfile()
		{
			CreateMap<Restaurant, RestaurantDto>()
				.ForMember(r => r.City, c => c.MapFrom(s => s.Address.City))
				.ForMember(r => r.Street, c => c.MapFrom(s => s.Address.Street))
				.ForMember(r => r.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

			CreateMap<Dish, DishDto>();

			CreateMap<CreateRestaurantDto, Restaurant>()
				.ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
				{
					City = dto.City,
					Street = dto.Street,
					PostalCode = dto.PostalCode
				}));

			CreateMap<CreateDishDto, Dish>();
		}
	}
}
