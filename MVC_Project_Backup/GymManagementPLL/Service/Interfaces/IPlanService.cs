using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlan();
        PlanViewModel? GetPlan(int PlanId);

        UpdatePlanViewModel? GetPlanToUpdate(int PlanId);

        bool UpdatePlane(int PlanId, UpdatePlanViewModel updatePlanViewModel);

        bool ToggleStatus(int Id);


    }
}
