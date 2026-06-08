using GymManagementDAL.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {

        public static bool SeedData(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {

            try
            {
                var HasUser = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();
                if (HasRoles && HasUser) return false;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new (){Name = "SuperAdmin"},
                        new (){Name = "Admin"}
                    };
                    foreach (var Role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(Role.Name!).Result)
                        {
                            roleManager.CreateAsync(Role).Wait();
                        }

                    }
                }
                if (!HasUser)
                {


                    var mainAdmin = new ApplicationUser
                    {
                        FirstName = "Aliata",
                        LastName = "Tarek",
                        UserName = "AliataTarek",
                        Email = "AliataTarek@gmail.com",
                        PhoneNumber = "01123569856",

                    };

                    userManager.CreateAsync(mainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();


                    var admin = new ApplicationUser
                    {
                        FirstName = "Mohamed",
                        LastName = "Omar",
                        UserName = "MohamedOmar",
                        Email = "MohamedOmar@gmail11.com",
                        PhoneNumber = "01123569886",
                    };

                    userManager.CreateAsync(admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                    


                }

                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Seed Failed {ex}");
                return false;
            }

        }
    }
}
