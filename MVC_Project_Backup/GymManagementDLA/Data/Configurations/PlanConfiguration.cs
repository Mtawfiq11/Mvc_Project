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
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {

            builder.Property(m => m.Name)
             .HasMaxLength(50)
             .HasColumnType("varchar");

            builder.Property(m => m.Description)
             .HasMaxLength(100)
             .HasColumnType("varchar");


            builder.Property(m => m.Price)
                .HasPrecision(10, 2);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationCK", "DurationDays Between 1 and 365");


            });


        }
    }
}
