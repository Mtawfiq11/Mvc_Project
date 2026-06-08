using GymManagementDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositorys.Interfaces
{
    public interface IUnitOfWork
    {
        ISessionRepository sessionRepository { get; }
        IMembershipRepository MembershipRepository { get; }
        IMemberSessionRepository MemberSessionRepository { get; }   // << new
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : BaseEntity, new();
        int SaveChanges();

    }
}
