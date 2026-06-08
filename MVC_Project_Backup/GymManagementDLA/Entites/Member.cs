using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entites
{
    public class Member : GymUser
    {
        public string photo { get; set; } = null!;

        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<MemberShip> MemberShips { get; set; } = null!;

        public ICollection<MemberSession> memberSessions { get; set; } = null!; 
    }
}
