using Shared.Entities;
using Microsoft.EntityFrameworkCore;



namespace Shared.DBContext
{
    public class DataDBContext : DbContext
    {
        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
        public DbSet<Doctors> Doctors { get; set; }

        public DataDBContext(DbContextOptions<DataDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctors>().HasKey(d => d.Id);
            modelBuilder.Entity<AvailabilitySlot>().HasKey(d => d.Id);
            modelBuilder.Entity<Patients>().HasKey(d => d.Id);
            modelBuilder.Entity<Appointments>().HasKey(d => d.Id);
            modelBuilder.Entity<DataLog>().HasKey(d => d.Id);
            modelBuilder.Entity<Statuses>().HasKey(d => d.Id);

            modelBuilder.Entity<Doctors>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AvailabilitySlot>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Patients>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointments>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DataLog>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Statuses>().Property(d => d.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<AvailabilitySlot>().Property(a => a.Time).IsRequired();
            modelBuilder.Entity<AvailabilitySlot>().Property(a => a.Cost).HasPrecision(18, 2);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection", options =>
                {
                    options.MigrationsAssembly("Shared"); // Set migration assembly
                });
            }
        }
    }
}
