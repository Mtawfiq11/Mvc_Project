using GymManagementDAL.Data.Context;
using GymManagementDAL.Entites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {

        public static bool SeedData (GymDbContext dbContext)
        {

            try
            {
                var HasPlan = dbContext.plans.Any();
                var Hascategories = dbContext.categories.Any();
                if (HasPlan && Hascategories) return false;

                if (!HasPlan)
                {
                    var Plan = LoadDataFromJsonFile<Plan>("plans.json");
                    if (Plan.Any())
                        dbContext.plans.AddRange(Plan);

                }

                if (!Hascategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                        dbContext.categories.AddRange(categories);

                }

                return dbContext.SaveChanges() > 0;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Faild {ex}");
                return false;
                
            }
             
        }



        private static List<T>LoadDataFromJsonFile<T>(string fileName)
        {
            // C: \Users\java\Desktop\route\mvc\mvc02\Gym Management SystemSolution\GymManagementPL\wwwroot\Files\categories.json

            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

            if (!File.Exists(FilePath)) throw new FileNotFoundException();
            string Data = File.ReadAllText(FilePath);
            var Option = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(Data , Option) ?? new List<T>();
        }

        

    }
}
