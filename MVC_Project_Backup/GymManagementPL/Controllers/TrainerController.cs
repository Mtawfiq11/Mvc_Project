using GymManagementBLL.Service.Classes;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainer
        public ActionResult Index()
        {

            var trainer = _trainerService.GetAllTrainers();
            return View(trainer);
        }
        #endregion


        #region Create trainer
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing fields");
                return View(nameof(Create), model);
            }
            var Result = _trainerService.CreateTrainer(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Create Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Field To Create";
            }
            return RedirectToAction(nameof(Index));


        }

        #endregion


        #region Details Trainer
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";

                return RedirectToAction(nameof(Index));
            }


            return View(trainer);
        }
        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));


            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing fields");
                return View(viewModel);
            }

            var Result = _trainerService.UpdateTrainerDetails(viewModel, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Update Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Field To Update";

            }

            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Delete Trainer
        public ActionResult Delete(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";

                return RedirectToAction(nameof(Index));

            }
            ViewBag.TrainerId = id;

            ViewBag.TrainerName = trainer.Name;

            return View();



        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Remove Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Not Deleted";
            }


            return RedirectToAction(nameof(Index));
        } 
        #endregion



    }
}
