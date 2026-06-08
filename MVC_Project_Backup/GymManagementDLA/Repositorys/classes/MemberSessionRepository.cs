using GymManagementDAL.Data.Context;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GymManagementDAL.Repositorys.classes
{
    public class MemberSessionRepository : IMemberSessionRepository
    {
        private readonly GymDbContext _context;

        public MemberSessionRepository(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MemberSession> GetBySessionId(int sessionId)
        {
            return _context.memberSessions
                .Include(ms => ms.Member)
                .Include(ms => ms.Session)
                .Where(ms => ms.SessionId == sessionId)
                .ToList();
        }

        public MemberSession? GetBySessionAndMember(int sessionId, int memberId)
        {
            return _context.memberSessions
                .FirstOrDefault(ms => ms.SessionId == sessionId && ms.MemberId == memberId);
        }

        public bool IsMemberBooked(int sessionId, int memberId)
        {
            return _context.memberSessions
                .Any(ms => ms.SessionId == sessionId && ms.MemberId == memberId);
        }

        public int GetBookingCount(int sessionId)
        {
            return _context.memberSessions.Count(ms => ms.SessionId == sessionId);
        }

        public void Add(MemberSession booking)
        {
            _context.memberSessions.Add(booking);
        }

        public void Delete(MemberSession booking)
        {
            _context.memberSessions.Remove(booking);
        }

    }
}
