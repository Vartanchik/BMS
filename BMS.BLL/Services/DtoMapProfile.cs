using AutoMapper;
using BMS.BLL.DTOs;
using BMS.DAL.Entities;

namespace BMS.BLL.Services
{
    public class DtoMapProfile : Profile
    {
        public DtoMapProfile()
        {
            CreateMap<AppUserDto, AppUser>()
                .ForMember(dest => dest.UserName,
                           config => config.MapFrom(src => src.Name));
            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Name,
                           config => config.MapFrom(src => src.UserName));
            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Post, PostDto>();
        }
    }
}
