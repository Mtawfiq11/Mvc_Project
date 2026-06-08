using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.Membership
{
    public class CreateMembershipViewModel
    {

        public int MemberId { get; set; }
        public int PlanId { get; set; }

        public IEnumerable<MemberSelectViewModel> Members { get; set; } = new List<MemberSelectViewModel>();
        public IEnumerable<PlanSelectViewModel> Plans { get; set; } = new List<PlanSelectViewModel>();
    }
}
