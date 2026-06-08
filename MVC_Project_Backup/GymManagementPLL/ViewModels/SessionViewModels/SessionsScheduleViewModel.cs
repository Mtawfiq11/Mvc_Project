using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class SessionsScheduleViewModel
    {

       
            public IEnumerable<SessionCardViewModel> UpcomingSessions { get; set; } = new List<SessionCardViewModel>();
            public IEnumerable<SessionCardViewModel> OngoingSessions { get; set; } = new List<SessionCardViewModel>();
        
    }
}
