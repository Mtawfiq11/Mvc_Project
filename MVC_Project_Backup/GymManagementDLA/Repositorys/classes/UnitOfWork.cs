using GymManagementDAL.Data.Context;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositorys.classes
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly Dictionary<Type, object> _repository = new();
        private readonly GymDbContext _dbContext;

        public ISessionRepository sessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IMemberSessionRepository MemberSessionRepository { get; }   

        public UnitOfWork(
            GymDbContext dbContext,
            ISessionRepository sessionRepository,
            IMembershipRepository membershipRepository,
            IMemberSessionRepository memberSessionRepository)
        {
            _dbContext = dbContext;
            this.sessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
            MemberSessionRepository = memberSessionRepository;
        }

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);

            if (_repository.TryGetValue(entityType, out var repo))
                return (IGenericRepository<TEntity>)repo;

            var newRepo = new GenericRepository<TEntity>(_dbContext);
            _repository[entityType] = newRepo;

            return newRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
