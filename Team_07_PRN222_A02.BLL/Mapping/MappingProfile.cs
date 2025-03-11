using AutoMapper;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<NewsArticle, NewsArticleDTO>()
                .ForMember(des => des.AccountName, act => act.MapFrom(src => src.CreatedBy.AccountName))
                .ForMember(des => des.Tags, act => act.MapFrom(src => src.Tags))
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName));

            CreateMap<Category, CategoryDTO>();


            CreateMap<NewsArticleUpdateDTO, NewsArticle>()
                .ForMember(dest => dest.NewsArticleId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
                
        }
    }
}
