using AutoMapper;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlan()
        {
            var plan = _unitOfWork.GenericRepository <Plan>().GetAll();
            if (plan == null || !plan.Any()) return [];
            var PlanViewModel= _mapper.Map<IEnumerable<PlanViewModel>> (plan); 
            return PlanViewModel;
            
            
        }

        public PlanViewModel? GetPlan(int PlanId)
        {
            var Plan = _unitOfWork.GenericRepository<Plan>().GetById(PlanId);
            if (Plan == null) return null;
            var planmodel = _mapper.Map<PlanViewModel> (Plan);
            return planmodel;
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = _unitOfWork.GenericRepository<Plan>().GetById(PlanId);
            if (Plan == null || Plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;

            return _mapper.Map<UpdatePlanViewModel> (Plan);

        }

        public bool ToggleStatus(int Id)
        {
            var Plan = _unitOfWork.GenericRepository<Plan>().GetById(Id);
            if (Plan == null || HasActiveMemberShip(Id)) return false;
            Plan.IsActive = Plan.IsActive ==  true ? false : true;
            Plan.UpdatedAt = DateTime.Now;

            try
            {
                _unitOfWork.GenericRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {

                return false;
            }
        }

        public bool UpdatePlane(int PlanId, UpdatePlanViewModel updatePlanViewModel)
        {
            var Plan = _unitOfWork.GenericRepository<Plan>().GetById(PlanId);
            if (Plan == null|| HasActiveMemberShip(PlanId)) return false;

            try
            {
                _mapper.Map(updatePlanViewModel, Plan);
                _unitOfWork.GenericRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {

                return false;

            }

        }

        private bool HasActiveMemberShip(int PlanId)
        {
            var ActiveMemberShip = _unitOfWork.GenericRepository<MemberShip>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active");
            return ActiveMemberShip.Any();

        }

    }
}
