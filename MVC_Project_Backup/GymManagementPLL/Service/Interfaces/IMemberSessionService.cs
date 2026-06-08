using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Interfaces
{
    public interface IMemberSessionService
    {
        IEnumerable<SessionCardViewModel> GetUpcomingSessions();
        IEnumerable<SessionCardViewModel> GetOngoingSessions();
        IEnumerable<MemberBookingViewModel> GetMembersBySession(int sessionId);
        bool BookMember(int sessionId, int memberId, out string errorMessage);
        bool CancelBooking(int sessionId, int memberId, out string errorMessage);
        bool MarkAsAttended(int bookingId, out string errorMessage);


    }
}
