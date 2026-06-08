using AutoMapper;
using GymManagementBLL.Service.AttachmentService;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.classes;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }
           


        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {

            try
            {
                var emailExists = _unitOfWork.GenericRepository<Member>().GetAll(x => x.Email == createMemberViewModel.Email).Any();
                var phoneExists = _unitOfWork.GenericRepository<Member>().GetAll(x => x.Phone == createMemberViewModel.Phone).Any();
                if (emailExists || phoneExists) return false;

                var photoName = _attachmentService.Upload("members", createMemberViewModel.PhotoFile);
                if(string.IsNullOrEmpty(photoName))return false;



                var Member = _mapper.Map<Member>(createMemberViewModel);
                Member.photo = photoName;

               _unitOfWork.GenericRepository<Member>().Add(Member) ;
                var IsCreate =  _unitOfWork.SaveChanges() > 0;
                if (!IsCreate)
                {
                    _attachmentService.Delete(photoName, "members");
                    return false;
                }
                else
                {
                    return IsCreate;
                }
            }
            catch (Exception )
            {
                return false;
            }
           



        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Member = _unitOfWork.GenericRepository<Member>().GetAll();
            if (Member == null || !Member.Any()) return [];

            var MemberViewModel = _mapper.Map<IEnumerable<MemberViewModel>>(Member);
            return MemberViewModel; 
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = _unitOfWork.GenericRepository<Member>().GetById(MemberId);
            if (Member == null) return null;
            var MemberModel = _mapper.Map<MemberViewModel>(Member);

            var ActiveMemberShip = _unitOfWork.GenericRepository<MemberShip>().GetAll(x => x.MemberId == MemberId && x.Status == "Active")
                                                .FirstOrDefault();

            if (ActiveMemberShip is not null)
            {
                var plan = _unitOfWork.GenericRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                MemberModel.MembershipStartDate = ActiveMemberShip.CreateAt.ToShortDateString();
                MemberModel.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                MemberModel.PlanName = plan?.Name;
            }

            return MemberModel;


        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
          var healthRecord = _unitOfWork.GenericRepository<HealthRecord>().GetById(MemberId);
            if (healthRecord == null) return null;
            return _mapper.Map<HealthRecordViewModel>(healthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GenericRepository<Member>().GetById(MemberId);
            if (Member == null) return null;
            return _mapper.Map<MemberToUpdateViewModel>(Member);
        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GenericRepository<Member>();

            var Member = MemberRepo.GetById(MemberId);
            if (Member == null) return false;

            var IdSession = _unitOfWork.GenericRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId).Select(x => x.SessionId);
            var HasFutureSession = _unitOfWork.GenericRepository<Session>()
                                              .GetAll(x => IdSession.Contains(x.Id) && x.StartDate > DateTime.Now).Any();


            if(HasFutureSession) return false;
            var MemberShip = _unitOfWork.GenericRepository<MemberShip>().GetAll(x => x.MemberId == MemberId);
            try
            {
                if (MemberShip.Any())
                {
                    foreach (var membership in MemberShip)
                    {
                        _unitOfWork.GenericRepository<MemberShip>().Delete(membership);
                    }

                }
                 MemberRepo.Delete(Member);
                var IsDelete = _unitOfWork.SaveChanges() > 0;
                if (IsDelete)
                
                    _attachmentService.Delete(Member.photo, "members");
                return IsDelete;
                

            }
            catch
            {
                return false;
            }


        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel memberUpdate)
        {

            try
            {
                var emailExists = _unitOfWork.GenericRepository<Member>().GetAll(x => x.Email == memberUpdate.Email && x.Id !=Id).Any();
                var phoneExists = _unitOfWork.GenericRepository<Member>().GetAll(x => x.photo == memberUpdate.Phone && x.Id != Id).Any();
                if (emailExists || phoneExists) return false;
                var Member  = _unitOfWork.GenericRepository<Member>().GetById(Id);
                if (Member == null) return false;
                _mapper.Map(memberUpdate, Member);

                _unitOfWork.GenericRepository<Member>().Update(Member)  ;

                return _unitOfWork.SaveChanges() > 0;





            }
            catch
            {

                return false;
            }


        }




    }
}
