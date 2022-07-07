using Auth.DataAccessLayer.Abstractions;
using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using Auth.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthContext _globalContext;

        public UnitOfWork(
            AuthContext context,
            IRepository<RefreshToken> authRepository,
            IUserRepository userRepository)
        {
            this._globalContext = context;

            authRepo = authRepository;
            userRepo = userRepository;
        }


        public IRepository<RefreshToken> authRepo { get; set; }
        public IUserRepository userRepo { get; set; }

        
        public void BeginTransaction()
        {
            _globalContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _globalContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {   
            _globalContext.Database.RollbackTransaction();
        }
      
        public void Complete()
        {
            _globalContext.SaveChanges();
        }

    }
}
