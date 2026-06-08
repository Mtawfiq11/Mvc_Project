using GymManagementBLL.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();
        bool CreateMembership(int memberId, int planId);
        IEnumerable<MemberSelectViewModel> GetMembers();
        IEnumerable<PlanSelectViewModel> GetPlans();
        void CancelMembership(int memberId, int planId);

    }
}
