using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class SessionCardViewModel
    {

        public int SessionId { get; set; }
        public string Description { get; set; } = "";
        public string TrainerName { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public string Status { get; set; } = "";
    }
}
