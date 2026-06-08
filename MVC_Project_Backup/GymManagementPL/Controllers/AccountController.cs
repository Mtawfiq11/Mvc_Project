using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.login;
using GymManagementDAL.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }


       public ActionResult Login()
        {
            return View();
        }

        #region Login


        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {

            if (!ModelState.IsValid) return View(viewModel);

            var user = _accountService.ValidateUser(viewModel);
            if (user == null)
            {
                ModelState.AddModelError("InvalidLogin", " Invalid Email Or Password ");
                return View(viewModel);
            }

            var Result = _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false).Result;
            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Is Not Allowed");
            if (Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Is LockedOut ");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(viewModel);

        }


        #endregion


        #region Logout




        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        } 

        #endregion


        public ActionResult AccessDenied()
        {

            return View();
            
        }


    }
}
