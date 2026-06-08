using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class MemberBookingViewModel
    {

        public int BookingId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = "";
        public DateTime BookingDate { get; set; }
        public bool IsAttended { get; set; }
    }
}
