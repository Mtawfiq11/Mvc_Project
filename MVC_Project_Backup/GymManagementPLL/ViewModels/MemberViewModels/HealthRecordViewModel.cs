using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is required")]
        [Range(0.1, 300, ErrorMessage = "Height must be greater than 0 and less than 300")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(0.1, 500, ErrorMessage = "Weight must be greater than 0 and less than 500")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Blood type is required")]
        [StringLength(3 , ErrorMessage = "Blood type must be 3 characters or less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }



    }
}
