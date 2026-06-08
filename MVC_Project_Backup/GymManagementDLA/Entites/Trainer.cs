using GymManagementDAL.Entites.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entites
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }

        public ICollection<Session> TrainerSession { get; set; } = null!;

    }
}
 