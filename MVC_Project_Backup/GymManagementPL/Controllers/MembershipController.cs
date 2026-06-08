using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.Membership;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
       
            private readonly IMembershipService _membershipService;

            public MembershipController(IMembershipService membershipService)
            {
                _membershipService = membershipService;
            }

            public IActionResult Index()
            {
                var memberships = _membershipService.GetAllMemberships();
                return View(memberships);
            }

            public IActionResult Create()
            {
              
        
            ViewBag.Members = _membershipService.GetMembers();
            ViewBag.Plans = _membershipService.GetPlans();
            return View();
        
            }

        [HttpPost]
        public IActionResult Create(int memberId, int planId)
        {
            bool result = _membershipService.CreateMembership(memberId, planId);

            if (!result)
            {
                TempData["Error"] = "This member already has an active membership.";
                return RedirectToAction("Create");
            }

            TempData["Success"] = "Membership created successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Cancel(int memberId, int planId)
        {
            _membershipService.CancelMembership(memberId, planId);
            TempData["SuccessMessage"] = "Membership cancelled successfully!";
            return RedirectToAction(nameof(Index));
        }



    }
}
