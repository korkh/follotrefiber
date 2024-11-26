using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<EmployeeTimeCard> EmployeeTimeCards { get; set; }
        public DbSet<WorkingState> WorkingStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Employee>()
                .HasMany(e => e.EmployeeTimeCards)
                .WithOne(tc => tc.Employee)
                .HasForeignKey(tc => tc.EmployeeId);

            builder.Entity<Employee>()
                .HasMany(e => e.BusinessTrips)
                .WithOne(bts => bts.Employee)
                .HasForeignKey(bts => bts.EmployeeId);

            builder.Entity<Employee>()
                .Property(e => e.BirthDay)
                .HasConversion(
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            builder.Entity<EmployeeTimeCard>()
                .HasMany(tc => tc.WorkingStates)
                .WithOne(ws => ws.EmployeeTimeCard)
                .HasForeignKey(ws => ws.EmployeeTimeCardId);

            builder
                .Entity<Role>()
                .HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Manager",
                        NormalizedName = "MANAGER"
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new Role
                    {
                        Id = 3,
                        Name = "Accountant",
                        NormalizedName = "ACCOUNTANT"
                    }
                );
            builder.Entity<Employee>(entity =>
            {
                entity.OwnsOne(
                    e => e.RegistrationAddress,
                    a =>
                    {
                        a.WithOwner();
                    }
                );
                entity.OwnsOne(
                    e => e.ContractData,
                    a =>
                    {
                        a.WithOwner();
                    }
                );
                
            });
            // builder.Entity<Document>().Property(d => d.RowVersion).IsRowVersion();
            // builder.Entity<Employee>().Property(d => d.RowVersion).IsRowVersion();
            // builder.Entity<Payroll>().Property(d => d.RowVersion).IsRowVersion();
        }
    }
}