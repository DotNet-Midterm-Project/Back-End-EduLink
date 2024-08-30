using EduLink.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : IdentityDbContext<User>
    {
        public EduLinkDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<WorkShop> WorkShops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring relationships between all Entities.

            //Relation Between Volunteers and Reservations (one to many) 
            modelBuilder.Entity<Reservation>()
                    .HasOne(r => r.Volunteer)
                    .WithMany(v => v.Reservations)
                    .HasForeignKey(r => r.VolunteerID);


            //Relation Between Volunteers and Workshops (one to many) 
            modelBuilder.Entity<WorkShop>()
                    .HasOne(w => w.Volunteer)
                    .WithMany(v => v.WorkShops)
                    .HasForeignKey(w => w.VolunteerID);

            //Relation Between Course and Reservation (one to many) 
            modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Course)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CourseID);

            // ٌRElation  Between WorkShop and Student(many-to-many)
            modelBuilder.Entity<WorkshopsRegistration>()
                .HasKey(wr => new { wr.WorkShopID, wr.StudentID });

            modelBuilder.Entity<WorkshopsRegistration>()
                .HasOne(wr => wr.WorkShop)
                .WithMany(w => w.WorkshopsRegistrations)
                .HasForeignKey(wr => wr.WorkShopID);

            modelBuilder.Entity<WorkshopsRegistration>()
                .HasOne(wr => wr.Student)
                .WithMany(s => s.WorkshopsRegistrations)
                .HasForeignKey(wr => wr.StudentID);

            //Relation between Reservation and Booking (one to one)
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Bookings)   // One booking can be associated with multiple bookings
                .WithOne(b => b.Reservation) // One booking is associated with one reservation
                .HasForeignKey(b => b.ReservationID)  
                .OnDelete(DeleteBehavior.NoAction);

            // Relation between Student and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Student)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.StudentID);

           

        }
    }
}
