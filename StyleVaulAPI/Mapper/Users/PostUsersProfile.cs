using AutoMapper;
using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Models;

namespace StyleVaulAPI.Mapper.Users
{
    public class PostUsersProfile : Profile
    {
        public PostUsersProfile()
        {
            CreateMap<PostUsers, User>().ReverseMap();
        }
    }
}
