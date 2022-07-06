using Auth.DataAccessLayer.Abstractions.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Abstractions
{
    public interface IUnitOfWork
    {
        public IAuthRepository authRepo { get; }
        public IUserRepository userRepo { get; }  

        public void BeginTransaction();
        public void CommitTransaction();
        public void RollbackTransaction();
        public void Complete();
    }
}
