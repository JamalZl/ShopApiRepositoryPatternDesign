using AutoMapper;
using Shop.Api.Apps.Admin.DTOs;
using Shop.Api.Apps.Admin.DTOs.ProductDtos;
using Shop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Apps.Admin.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryInProductGetDto>();

            CreateMap<Product, ProductGetDto>()
                .ForMember(dest => dest.Profit, map => map.MapFrom(src => src.SalePrice - src.CostPrice));

        }
    }
}
