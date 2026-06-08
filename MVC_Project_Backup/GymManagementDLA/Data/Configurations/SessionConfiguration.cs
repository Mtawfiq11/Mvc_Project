using GymManagementDAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {

                tb.HasCheckConstraint("SessionCapacityCk", "Capacity Between 1 and 25 ");
                tb.HasCheckConstraint("SessionEndDateCk", "EndDate > StartDate ");

            });

            builder.HasOne(x => x.SessionCategory)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.SessionTrainer)
                .WithMany(x => x.TrainerSession)
                .HasForeignKey(x => x.TrainerId);


        }
    }
}
