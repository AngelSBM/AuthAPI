using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PublicId { get; set; }

        #region Relationships
        public ICollection<UserRole> UsersRoles { get; set; }
        #endregion
    }
}
