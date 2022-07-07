using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.DTOs
{
    public class UserDetailDTO : UserDTO 
    {
        public List<RoleDTO> Roles { get; set; }
    }
}
