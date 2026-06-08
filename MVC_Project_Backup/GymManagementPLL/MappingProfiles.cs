using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entites;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            MapSession();

            MapMember();
            MapTrainer();
            MapPlan();

        }
        
       private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(s => s.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, x => x.MapFrom(s => s.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlots, x => x.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }

        private void MapMember()
        {
            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(d => d.Address, opt => opt.MapFrom(s => new Address
            //    {
            //        BuildingNumber = s.BuildingNumber,
            //        City = s.City,
            //        Street = s.Street,

            //    }));


            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s))
                .ForMember(d=>d.HealthRecord,opt=>opt.MapFrom(s=>s.healthRecordViewModel));
            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(d => d.BuildingNumber, opt => opt.MapFrom(s => s.BuildingNumber))
                .ForMember(d => d.City, opt => opt.MapFrom(s => s.City))
                .ForMember(d => d.Street, opt => opt.MapFrom(s => s.Street));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(d=>d.Gender,opt=>opt.MapFrom(s=>s.Gender.ToString()))
                .ForMember(d=>d.DateOfBirth,p=>p.MapFrom(s=>s.DateOfBirth.ToShortDateString()))
                .ForMember(d=>d.Address , p => p.MapFrom(s =>$"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"));


            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(d => d.BuildingNumber, p => p.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(d => d.Street, p => p.MapFrom(s => s.Address.Street))
                .ForMember(d => d.City, p => p.MapFrom(s => s.Address.City));


            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(d => d.Name, p => p.Ignore())
                .ForMember(d => d.photo, p => p.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Address.BuildingNumber = s.BuildingNumber;
                    d.Address.Street = s.Street;
                    d.Address.City = s.City;
                    d.UpdatedAt = DateTime.Now;


                });



           

        }

        private void MapTrainer()
        {

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Trainer, TrainerViewModel>()
      .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties.ToString()))
      .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
      .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString()))
      .ForMember(dest => dest.Address, opt => opt.MapFrom(s => $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"));

      





            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });



        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(d=>d.PlanName,p=>p.MapFrom(s=>s.Name));

            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }


    }
}
