using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MarketPlaceDAL;
using MarketPlaceMVC;
namespace MarketPlaceMVC.Repository
{

    public class MarketPlaceMapper : Profile
    {
    
        public  MarketPlaceMapper()
        {
           CreateMap<User, Models.User>();
           CreateMap<Item, Models.Item>();
           CreateMap<Category, Models.Category>();
           CreateMap<SubCategory, Models.SubCategory>();
           CreateMap<Models.User, User>();
           CreateMap<Models.Category, Category>();
           CreateMap<Models.SubCategory, SubCategory>();
           CreateMap<Models.Item, Item>();

        }

    }
}