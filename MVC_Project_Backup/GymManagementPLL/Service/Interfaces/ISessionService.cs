using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSession();
        SessionViewModel? GetSessionById(int Id);


        bool CreateSession(CreateSessionViewModel createSession);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        bool UpdateSession(UpdateSessionViewModel UpdateSession, int sessionId);

        bool RemoveSession(int sessionId);

        IEnumerable<TrainerSelectViewModel> GetTrainer();
        IEnumerable<CategorySelectViewModel> GetCategory();


    }
}
