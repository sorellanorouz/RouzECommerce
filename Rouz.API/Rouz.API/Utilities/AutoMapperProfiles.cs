using AutoMapper;
using Rouz.API.DTOs;
using Rouz.API.Entities;

namespace Rouz.API.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Usuario
            this.CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
