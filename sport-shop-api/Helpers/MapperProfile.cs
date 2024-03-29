﻿using AutoMapper;
using sport_shop_api.Models.DTOs;
using sport_shop_api.Models.Entities;

namespace sport_shop_api.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ProductFileDTO, Product>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductSize, ProductSizeDTO>().ReverseMap();
            CreateMap<HistoryProductDTO, HistoryProduct>();
        }
    }
}
