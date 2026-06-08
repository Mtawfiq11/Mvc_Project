using AutoMapper;
using GymManagementBLL.Service.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entites;
using GymManagementDAL.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymManagementBLL.Service.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {

            try
            {
                var Repo = _unitOfWork.GenericRepository<Trainer>();
                var emailExists = Repo.GetAll(x => x.Email == createdTrainer.Email).Any();
                var phoneExists = Repo.GetAll(x => x.Phone == createdTrainer.Phone).Any();
                if (emailExists || phoneExists) return false;
                var Trainer = _mapper.Map<Trainer>(createdTrainer);

                Repo.Add(Trainer);
                return _unitOfWork.SaveChanges() > 0;
            }


            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {

            var Trainer = _unitOfWork.GenericRepository<Trainer>().GetAll();
            if (Trainer == null || !Trainer.Any()) return [];

            var Trainer01 = _mapper.Map<IEnumerable<TrainerViewModel>>(Trainer);
            return Trainer01;





        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GenericRepository<Trainer>().GetById(trainerId);
            if (Trainer == null) return null;
            var trainer = _mapper.Map<TrainerViewModel>(Trainer);
            return trainer;


        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {

            var Trainer = _unitOfWork.GenericRepository<Trainer>().GetById(trainerId);
            if (Trainer == null) return null;
            return _mapper.Map<TrainerToUpdateViewModel>(Trainer);



        }

        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GenericRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove == null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;


        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {

            var Repo = _unitOfWork.GenericRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(trainerId);
            var emailExists = Repo.GetAll(x => x.Email == updatedTrainer.Email && x.Id != trainerId).Any();
            var phoneExists = Repo.GetAll(x => x.Phone == updatedTrainer.Phone && x.Id != trainerId).Any();
            if (TrainerToUpdate == null || emailExists || phoneExists) return false;
            _mapper.Map(updatedTrainer, TrainerToUpdate);
            Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;

        }



        private bool HasActiveSessions(int Id)
        {
            var activeSession = _unitOfWork.GenericRepository<Session>().GetAll
                (x => x.TrainerId == Id && x.StartDate > DateTime.Now).Any();
            return activeSession;

        }
    }

}