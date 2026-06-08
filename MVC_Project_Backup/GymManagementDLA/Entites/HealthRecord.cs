using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entites
{
    public class HealthRecord :BaseEntity
    {
        public double Height { get; set; }    
        public double Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }

    }
}
