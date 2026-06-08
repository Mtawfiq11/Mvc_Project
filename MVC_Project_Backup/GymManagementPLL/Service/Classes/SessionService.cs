using AutoMapper;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.classes;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementBLL.Service.Classes
{
    public class SessionService : ISessionService
    {
       
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public bool CreateSession(CreateSessionViewModel createSession)
            {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;
                if (createSession.Capacity > 25 || createSession.Capacity < 0) return false;
                var sessionEntity = _mapper.Map<Session>(createSession);
                _unitOfWork.GenericRepository<Session>().Add(sessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }


        }

        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var sessions = _unitOfWork.sessionRepository.GetAllSessionWithTrainerAndCategory();
            if (!sessions.Any()) return [];
            //return sessions.Select(session => new SessionViewModel
            //{

            //       Id = session.Id,
            //       Description = session.Description,
            //       StartDate = session.StartDate,
            //       EndDate = session.EndDate,
            //       Capacity = session.Capacity,
            //       TrainerName = session.SessionTrainer.Name,
            //       CategoryName = session.SessionCategory.CategoryName,
            //       AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBokedSlots(session.Id), 


            //});

            var mapsesstion = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            foreach (var s in mapsesstion)
                s.AvailableSlots = s.Capacity - _unitOfWork.sessionRepository.GetCountOfBokedSlots(s.Id);
            

            return mapsesstion;


        }

        public SessionViewModel? GetSessionById(int Id)
        {
            var session = _unitOfWork.sessionRepository.GetSessionWithTrainerAndCategory(Id);
            if (session == null) return null;

            var mapsesstion = _mapper.Map<Session, SessionViewModel>(session);
           mapsesstion.AvailableSlots = mapsesstion.Capacity - _unitOfWork.sessionRepository.GetCountOfBokedSlots(mapsesstion.Id);
            return mapsesstion;

        }


        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.sessionRepository.GetById(sessionId);
            if (!IsSessionAvailableForupdate(session!)) return null;
          return  _mapper.Map<UpdateSessionViewModel>(session);
        }


        public bool UpdateSession(UpdateSessionViewModel UpdateSession, int sessionId)
        {
            try
            {
                var session = _unitOfWork.sessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForupdate(session!)) return false;
                if (!IsTrainerExists(UpdateSession.TrainerId)) return false;
                if (!IsDateTimeValid(UpdateSession.StartDate, UpdateSession.EndDate)) return false;
                _mapper.Map(UpdateSession, session);
                session.UpdatedAt = DateTime.Now;
                _unitOfWork.sessionRepository.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Is Failed{ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
           
            try
            {
                var session = _unitOfWork.sessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForremove(session!)) return false;
                _unitOfWork.sessionRepository.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Is Failed{ex}");
                return false;
            }


        }

        public IEnumerable<TrainerSelectViewModel> GetTrainer()
        {
          var trainer =  _unitOfWork.GenericRepository<Trainer>().GetAll();
            return trainer.Select(t => new TrainerSelectViewModel
            {
                Id = t.Id,
                Name = t.Name,

            });


        }
        public IEnumerable<CategorySelectViewModel> GetCategory()
        {
            var Category = _unitOfWork.GenericRepository<Category>().GetAll();
            return Category.Select(t => new CategorySelectViewModel
            {
                Id = t.Id,
                Name = t.CategoryName,

            });
        }

        #region Helper Methods

        private bool IsSessionAvailableForupdate(Session session)
        {
            if (session is null) return false;
            if(session.EndDate < DateTime.Now) return false;
            if(session.StartDate <= DateTime.Now) return false;
            var HasActiveBoking = _unitOfWork.sessionRepository.GetCountOfBokedSlots(session.Id) > 0;
            if (HasActiveBoking) return false;
            return true;


        }

        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GenericRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GenericRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate && DateTime.Now < StartDate;
        }

        private bool IsSessionAvailableForremove(Session session)
        {
            if (session == null) return false;
           
            if (session.StartDate <= DateTime.Now  && session.EndDate > DateTime.Now) return false;
            if (session.StartDate > DateTime.Now) return false;
            var HasActiveBoking = _unitOfWork.sessionRepository.GetCountOfBokedSlots(session.Id) > 0;
            if (HasActiveBoking) return false;
            return true;


        }

       
        #endregion
    }
}
