using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService )
        {
            _memberService = memberService;
        }

        #region Get All Member
        public ActionResult Index()
        {
            var member = _memberService.GetAllMembers();
            return View(member);
        }
        #endregion

        #region Get Member Data

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
               
            var mamber = _memberService.GetMemberDetails(id);
            if (mamber == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";

                return RedirectToAction(nameof(Index));
            }
                

            return View(mamber);




        }

        public ActionResult HealthRecordDetails(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] ="Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));


            }
            var mamber = _memberService.GetMemberHealthRecordDetails(id);
            if (mamber == null)
            {
                TempData["ErrorMessage"] ="Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
                

            return View(mamber);

        }


        #endregion

        #region Create Member
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMemberView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields");
                return View(nameof(Create) , createMemberView);

            }

            var Result =  _memberService.CreateMember(createMemberView);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Create Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Field To Create";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));


            }
            var mamber = _memberService.GetMemberToUpdate(id);
            if (mamber == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(mamber);
        }



        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id,MemberToUpdateViewModel memberToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(memberToUpdate);
            }
           
            var Result = _memberService.UpdateMemberDetails(id, memberToUpdate);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Update Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Member Field To Update";

            }

            return RedirectToAction(nameof(Index));

        }



        #endregion

        #region Member Delete
        public ActionResult Delete(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";

                return RedirectToAction(nameof(Index));

            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;

            return View();



        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Update Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Not Deleted";
            }

            return RedirectToAction(nameof(Index));
        } 
        #endregion



    }
}
