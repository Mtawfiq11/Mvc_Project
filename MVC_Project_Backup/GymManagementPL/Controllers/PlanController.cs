using GymManagementBLL.Service.Classes;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]

    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }


        #region Get All Plan
        public ActionResult Index()
        {
            var plan = _planService.GetAllPlan();

            return View(plan);
        }
        #endregion


        #region Get Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Plan Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlan(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "plan Not Found";

                return RedirectToAction(nameof(Index));
            }


            return View(plan);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Plan Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));


            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = " Plan Can Not Update";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }


        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Missing fields");
                return View(viewModel);
            }

            var Result = _planService.UpdatePlane(id, viewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Update Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Plan Field To Update";

            }

            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region Activate Plan

        [HttpPost]
        public ActionResult Activate(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Plan Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.ToggleStatus(id);
            if (plan)
            {
                TempData["SuccessMessage"] = "Plan Status Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Deleted Fields";
            }
            return RedirectToAction(nameof(Index));

        } 
        #endregion



    }
}
