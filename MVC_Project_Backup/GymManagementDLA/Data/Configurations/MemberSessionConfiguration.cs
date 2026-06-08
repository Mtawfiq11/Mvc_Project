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
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {

            builder.Property(x => x.CreateAt)
              .HasColumnName("StartDate")
              .HasDefaultValueSql("GETDATE()");
            builder.HasKey(x => new { x.MemberId, x.SessionId });

            builder.Ignore(x => x.Id);




        }
    }
}
