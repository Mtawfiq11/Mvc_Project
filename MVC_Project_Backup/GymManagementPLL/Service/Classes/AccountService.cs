using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.login;
using GymManagementDAL.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (user is null) return null;
           var IsPasswordValid = _userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
            return IsPasswordValid? user : null;



        }
    }
}
