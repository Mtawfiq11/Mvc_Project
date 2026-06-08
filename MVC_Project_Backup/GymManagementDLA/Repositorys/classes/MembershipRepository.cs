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
    public class MembershipRepository : IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MemberShip> GetAllWithIncludes()
        {
            return _context.memberShips
                .Include(m => m.Member)
                .Include(m => m.Plan)
                .ToList();
        }

        public MemberShip? GetMembership(int memberId, int planId)
        {
            return _context.memberShips
                .FirstOrDefault(m => m.MemberId == memberId && m.PlanId == planId);
        }

        public bool HasActiveMembership(int memberId)
        {
            return _context.memberShips
                .Any(m => m.MemberId == memberId && m.EndDate > DateTime.Now);
        }

        public void Add(MemberShip membership)
        {
            _context.memberShips.Add(membership);
        }

        public void Delete(int memberId, int planId)
        {
            var membership = GetMembership(memberId, planId);
            if (membership != null)
                _context.memberShips.Remove(membership);
        }

    }
}
