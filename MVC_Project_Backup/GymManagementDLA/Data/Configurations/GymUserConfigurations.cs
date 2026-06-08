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
    internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(m => m.Name)
             .HasMaxLength(50)
             .HasColumnType("varchar");

            builder.Property(m => m.Email)
             .HasMaxLength(100)
             .HasColumnType("varchar");

            builder.Property(m => m.Phone)
               .HasMaxLength(11)
              .HasColumnType("varchar");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("GymUserValidEmailCK", "[Email] LIKE '_%@_%._%'");

                t.HasCheckConstraint("GymUserValidPhoneCK",
                    " [Phone] LIKE '010[0-9]%' OR [Phone] LIKE '011[0-9]%' OR [Phone] LIKE '012[0-9]%' OR [Phone] LIKE '015[0-9]%'  AND LEN([Phone]) = 11");

            });


            builder.HasIndex(x=>x.Email).IsUnique();
            builder.HasIndex(x=>x.Phone).IsUnique();


            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {

                AddressBuilder.Property(x => x.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AddressBuilder.Property(x => x.City)
               .HasColumnName("City")
               .HasColumnType("varchar")
               .HasMaxLength(30);

                AddressBuilder.Property(x => x.BuildingNumber)
                .HasColumnName("BuildingNumber"); 




            });
           
        }
    }
}
