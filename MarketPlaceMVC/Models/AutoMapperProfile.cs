using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
namespace MarketPlaceMVC.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, Models.User>();
            CreateMap<Item, Models.Item>();
            CreateMap<Category, Models.Category>();
            CreateMap<SubCategory, Models.SubCategory>();
            CreateMap<Models.User, User>();
            CreateMap<Models.Category, Category>();
            CreateMap<Models.SubCategory, SubCategory>();
            CreateMap<Models.Item, Item>();
            CreateMap<Models.Cart, Cart>();
            CreateMap<Cart, Models.Cart>();
            CreateMap<Models.Order, Order>();
            CreateMap<Order, Models.Order>();

        }
    }
}