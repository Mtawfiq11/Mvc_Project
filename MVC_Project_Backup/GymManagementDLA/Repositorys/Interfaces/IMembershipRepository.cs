using GymManagementDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositorys.Interfaces
{
    public interface IMembershipRepository
    {

        IEnumerable<MemberShip> GetAllWithIncludes();
        MemberShip? GetMembership(int memberId, int planId);
        bool HasActiveMembership(int memberId);
        void Add(MemberShip membership);
        void Delete(int memberId, int planId);
    }
}
