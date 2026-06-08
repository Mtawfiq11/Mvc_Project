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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository (GymDbContext dbContext) : base (dbContext)
        {
           _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
            return _dbContext.sessions.Include(x => x.SessionTrainer)
                                 .Include(x => x.SessionCategory)
                                 .ToList();
        }
        public int GetCountOfBokedSlots(int sessionId)
        {

           return _dbContext.memberSessions.Count(x => x.SessionId == sessionId);

        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.sessions.Include (x => x.SessionTrainer)
                                      .Include (x => x.SessionCategory)
                                      .FirstOrDefault(x=>x.Id == sessionId);
        }
    }
}
