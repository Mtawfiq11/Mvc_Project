using GymManagementBLL.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberSessionController : Controller
    {


        private readonly IMemberSessionService _service;

        public MemberSessionController(IMemberSessionService service)
        {
            _service = service;
        }




        public IActionResult Upcoming()
        {
            var model = _service.GetUpcomingSessions();
            return View(model);
        }

        public IActionResult Ongoing()
        {
            var model = _service.GetOngoingSessions();
            return View(model);
        }

        public IActionResult Members(int sessionId)
        {
            var model = _service.GetMembersBySession(sessionId);
            return View(model);
        }

        [HttpPost]
        public IActionResult Book(int sessionId, int memberId)
        {
            if (!_service.BookMember(sessionId, memberId, out string error))
                TempData["Error"] = error;
            else
                TempData["Success"] = "Member booked successfully!";
            return RedirectToAction("Upcoming");
        }

        [HttpPost]
        public IActionResult Cancel(int sessionId, int memberId)
        {
            if (!_service.CancelBooking(sessionId, memberId, out string error))
                TempData["Error"] = error;
            else
                TempData["Success"] = "Booking canceled successfully!";
            return RedirectToAction("Members", new { sessionId });
        }

        [HttpPost]
        public IActionResult Mark(int bookingId)
        {
            if (!_service.MarkAsAttended(bookingId, out string error))
                TempData["Error"] = error;
            else
                TempData["Success"] = "Attendance marked successfully!";
            return RedirectToAction("Ongoing");
    }   }
}
