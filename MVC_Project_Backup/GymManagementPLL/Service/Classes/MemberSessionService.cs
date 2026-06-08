using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class MemberSessionService : IMemberSessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberSessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SessionCardViewModel> GetUpcomingSessions()
        {
            var sessions = _unitOfWork.sessionRepository.GetAll(s => s.StartDate > DateTime.Now);
            return sessions.Select(s => MapSessionToCard(s));
        }

        public IEnumerable<SessionCardViewModel> GetOngoingSessions()
        {
            var sessions = _unitOfWork.sessionRepository.GetAll(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now);
            return sessions.Select(s => MapSessionToCard(s));
        }

        public IEnumerable<MemberBookingViewModel> GetMembersBySession(int sessionId)
        {
            return _unitOfWork.MemberSessionRepository.GetBySessionId(sessionId)
                .Select(ms => new MemberBookingViewModel
                {
                    BookingId = ms.Id,
                    MemberId = ms.MemberId,
                    MemberName = ms.Member.Name,
                    BookingDate = ms.CreateAt,
                    IsAttended = ms.IsAttended
                });
        }

        public bool BookMember(int sessionId, int memberId, out string errorMessage)
        {
            errorMessage = "";
            var session = _unitOfWork.sessionRepository.GetById(sessionId);
            if (session == null) { errorMessage = "Session not found."; return false; }

            if (session.StartDate <= DateTime.Now) { errorMessage = "Can only book upcoming sessions."; return false; }

            if (_unitOfWork.MemberSessionRepository.GetBookingCount(sessionId) >= session.Capacity)
            { errorMessage = "Session is full."; return false; }

            if (_unitOfWork.MemberSessionRepository.IsMemberBooked(sessionId, memberId))
            { errorMessage = "Member already booked."; return false; }

            _unitOfWork.MemberSessionRepository.Add(new MemberSession
            {
                MemberId = memberId,
                SessionId = sessionId,
                IsAttended = false
            });
            _unitOfWork.SaveChanges();
            return true;
        }

        public bool CancelBooking(int sessionId, int memberId, out string errorMessage)
        {
            errorMessage = "";
            var booking = _unitOfWork.MemberSessionRepository.GetBySessionAndMember(sessionId, memberId);
            if (booking == null) { errorMessage = "Booking not found."; return false; }

            var session = _unitOfWork.sessionRepository.GetById(sessionId);
            if (session.StartDate <= DateTime.Now) { errorMessage = "Cannot cancel ongoing session."; return false; }

            _unitOfWork.MemberSessionRepository.Delete(booking);
            _unitOfWork.SaveChanges();
            return true;
        }

        public bool MarkAsAttended(int bookingId, out string errorMessage)
        {
            errorMessage = "";
            var booking = _unitOfWork.GenericRepository<MemberSession>().GetById(bookingId);
            if (booking == null) { errorMessage = "Booking not found."; return false; }

            var session = _unitOfWork.sessionRepository.GetById(booking.SessionId);
            if (!(session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now))
            { errorMessage = "Can only mark attendance during session."; return false; }

            booking.IsAttended = true;
            _unitOfWork.GenericRepository<MemberSession>().Update(booking);
            _unitOfWork.SaveChanges();
            return true;
        }

        private SessionCardViewModel MapSessionToCard(Session s)
        {
            var bookedCount = _unitOfWork.MemberSessionRepository.GetBookingCount(s.Id);
            var status = s.StartDate > DateTime.Now ? "Upcoming" : "Ongoing";

            return new SessionCardViewModel
            {
                SessionId = s.Id,
                Description = s.Description,
                TrainerName = s.SessionTrainer?.Name ?? "",
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                Booked = bookedCount,
                Status = status
            };

    }       }
}
