using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Session = _unitOfWork.GenericRepository<Session>().GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GenericRepository<MemberShip>().GetAll(x => x.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GenericRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GenericRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = Session.Count(x => x.StartDate > DateTime.Now),
                OngoingSessions = Session.Count(x=>x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now),
                CompletedSessions = Session.Count(x=>x.EndDate < DateTime.Now)
                

            };
        }
    }
}
