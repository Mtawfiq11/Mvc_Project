using GymManagementDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositorys.Interfaces
{
    public interface IMemberSessionRepository
    {
        IEnumerable<MemberSession> GetBySessionId(int sessionId);
        MemberSession? GetBySessionAndMember(int sessionId, int memberId);
        bool IsMemberBooked(int sessionId, int memberId);
        int GetBookingCount(int sessionId);
        void Add(MemberSession booking);
        void Delete(MemberSession booking);

    }
}
