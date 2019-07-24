using AutoMapper;
using BMS.BLL.DTOs;
using BMS.WebAPI.Models;

namespace BMS.WebAPI
{
    public class ModelMapProfile : Profile
    {
        public ModelMapProfile()
        {
            CreateMap<RegisterModel, AppUserDto>();
            CreateMap<PostCreateModel, PostDto>();
            CreateMap<PostUpdateModel, PostDto>();
            CreateMap<CommentCreateModel, CommentDto>();
            CreateMap<CommentUpdateModel, CommentDto>();
        }
    }
}
