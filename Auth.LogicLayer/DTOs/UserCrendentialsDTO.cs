using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.DTOs
{
    public class UserCrendentialsDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
