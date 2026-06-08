using GymManagementBLL.Service.Classes;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]

    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        #region Get All Session

        public ActionResult Index()
        {
            var session = _sessionService.GetAllSession();
            return View(session);
        }

        #endregion

        #region Details Session
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Session Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";

                return RedirectToAction(nameof(Index));
            }


            return View(session);
        }
        #endregion

        #region Create Session
        public ActionResult Create()
        {
            loadCategoryData();
            loadTrainerData();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                loadCategoryData();
                loadTrainerData();
                return View(model);
            }
            var Result = _sessionService.CreateSession(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Create Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Session Field To Create";
                return RedirectToAction(nameof(Index));
            }
           


        }

        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Session Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));


            }
            var session = _sessionService.GetSessionToUpdate(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Can Not Edit";
                return RedirectToAction(nameof(Index));
            }
            loadTrainerData();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                loadTrainerData();
                return View(viewModel);
            }

            var Result = _sessionService.UpdateSession(viewModel, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Update Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Session Field To Update";
                return RedirectToAction(nameof(Index));

            }
            loadTrainerData();
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Delete
        public ActionResult Delete(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Session Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var sessoin = _sessionService.GetSessionById(id);
            if (sessoin is null) 
            { 
              
                TempData["ErrorMessage"] = "Session Not Found";

                return RedirectToAction(nameof(Index));

            }
            ViewBag.SessionId  = id;          

            return View();



        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Remove Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session cant Not Deleted";
            }


            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Helper

        private void loadCategoryData()
        {

            var Category = _sessionService.GetCategory();
            ViewBag.Category = new SelectList(Category, "Id", "Name");
            
        }
        private void loadTrainerData()
        {

            
            var trainer = _sessionService.GetTrainer();
            ViewBag.Trainer = new SelectList(trainer, "Id", "Name");
        }


        #endregion

    }
}
