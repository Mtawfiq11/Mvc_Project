using GymManagementDAL.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Context
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {

        }
        

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server=.;Database=GymSystem;Trusted_Connection=True ;TrustServerCertificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(ap =>
            {
                ap.Property(x => x.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            });
            modelBuilder.Entity<ApplicationUser>(ap =>
            {
                ap.Property(x => x.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            });

        }



       
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> healthRecords { get; set; }
        public DbSet<Trainer> Trainers  { get; set; }
        public DbSet<Plan> plans  { get; set; }
        public DbSet<Category> categories   { get; set; }
        public DbSet<Session> sessions  { get; set; }
        public DbSet<MemberShip> memberShips  { get; set; }
        public DbSet<MemberSession> memberSessions { get; set; }
       



    }

}
