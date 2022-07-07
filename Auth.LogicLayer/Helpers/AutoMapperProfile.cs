using Auth.DataAccessLayer.Entities;
using Auth.LogicLayer.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, UserDetailDTO>()
                .ForMember(x => x.Roles, options => options.MapFrom(MapUserRole)).ReverseMap();
        }

        private List<RoleDTO> MapUserRole(User user, UserDetailDTO userDetailDTO)
        {

            var roles = new List<RoleDTO>();
            if(user.UsersRoles == null) {  return roles; }
            foreach (var userRole in user.UsersRoles)
            {
                roles.Add(new RoleDTO
                {
                    Id = userRole.Role.PublicId,
                    Name = userRole.Role.Name,
                    Description = userRole.Role.Description,
                });
            }

            return roles;

        }

    }
}
