using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.Membership;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementBLL.Service.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembershipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CancelMembership(int memberId, int planId)
        {
            _unitOfWork.MembershipRepository.Delete(memberId, planId);
            _unitOfWork.SaveChanges();
        }

        public bool CreateMembership(int memberId, int planId)
        {
            bool hasActiveMembership = _unitOfWork.MembershipRepository.HasActiveMembership(memberId);

            if (hasActiveMembership)
                return false;

            var plan = _unitOfWork.GenericRepository<Plan>().GetById(planId);

            var membership = new MemberShip
            {
                MemberId = memberId,
                PlanId = planId,
                EndDate = DateTime.Now.AddDays(plan.DurationDays),
                CreateAt = DateTime.Now
            };

            _unitOfWork.MembershipRepository.Add(membership);
            _unitOfWork.SaveChanges();

            return true;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MembershipRepository.GetAllWithIncludes();

            return memberships.Select(m => new MembershipViewModel
            {
                MemberId = m.MemberId,
                PlanId = m.PlanId,
                MemberName = m.Member.Name,
                PlanName = m.Plan.Name,
                StartDate = m.CreateAt,
                EndDate = m.EndDate,
                Status = m.Status
            });
        }

        public IEnumerable<MemberSelectViewModel> GetMembers()
        {
            return _unitOfWork.GenericRepository<Member>()
                .GetAll()
                .Select(m => new MemberSelectViewModel { Id = m.Id, Name = m.Name });
        }

        public IEnumerable<PlanSelectViewModel> GetPlans()
        {
            return _unitOfWork.GenericRepository<Plan>()
                .GetAll()
                .Select(p => new PlanSelectViewModel { Id = p.Id, Name = p.Name });
        }
    }
}
