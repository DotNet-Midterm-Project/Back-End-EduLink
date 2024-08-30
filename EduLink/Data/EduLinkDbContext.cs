using EduLink.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Department_Courses> Department_courses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Notification_Booking> NotificationBookings { get; set; }
        public DbSet<Booking> Bookings  { get; set; } 

        public EduLinkDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<WorkShop> WorkShops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring relationships between all Entities.



           
            modelBuilder.Entity<User>().HasKey(u => u.Id);  // Set primary key for User entity
            
            modelBuilder.Entity<Student>().HasKey(u => u.StudentID); // Set primary key for Student entity
            modelBuilder.Entity<Department>().HasKey(u => u.DepartmentID); // Set primary key for Department entity
          
            modelBuilder.Entity<Course>().HasKey(x => x.CourseID);  // Set primary key for Course entity
            modelBuilder.Entity<Admin>().HasKey(x => x.AdminID); // Set primary key for Admin entity
          
            modelBuilder.Entity<Notification_Booking>().HasKey(x => x.NotificationID); // Set primary key for Notification_Booking entity 

            // Define One-To-One relationship between User and Student
            modelBuilder.Entity<Student>()
               .HasOne(x => x.User)
               .WithOne(x => x.Student)
               .HasForeignKey<Student>(x => x.StudentID)
               .OnDelete(DeleteBehavior.NoAction); ;
            // End One-To-One relationship between User and Student

            // Define One-To-One relationship between User and Admin
            modelBuilder.Entity<Admin>()
               .HasOne(x => x.User)
               .WithOne(x => x.Admin)
               .HasForeignKey<Admin>(x => x.AdminID)
               .OnDelete(DeleteBehavior.NoAction);
            // End One-To-One relationship between User and Admin

            // Define One-To-Many relationship between Department and Student
             modelBuilder.Entity<Student>()
               .HasOne(x => x.Department)
               .WithMany(x => x.Students)
               .HasForeignKey(x => x.DepartmentID)
               .OnDelete(DeleteBehavior.SetNull);
            // End One-To-Many relationship between Department and Student

            // Define Many-To-Many relationship between Department and Courses
            modelBuilder.Entity<Department_Courses>().HasKey(x => new { x.CourseID, x.DepartmentID }); // Set composite key for Department_Courses entity
            modelBuilder.Entity<Department_Courses>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Department_Courses)
                .HasForeignKey(x => x.DepartmentID);
                
            

           
            modelBuilder.Entity<Department_Courses>()
                .HasOne(x => x.Course)
                .WithMany(x => x.Department_Courses)
                .HasForeignKey(x => x.CourseID);
            // End Many-To-Many relationship between Course and Department


            // Define One-To-Many relationship between Notification_Booking and Student
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.Student)
                .WithMany(x => x.Notification_Bookings)
                .HasForeignKey(x => x.StudentID);
            // End One-To-Many relationship etween Notification_Booking and Student

            // Define One-To-Many relationship between Notification_Booking and Booking
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.booking)
                .WithMany(x => x.notification_Bookings)
                .HasForeignKey(x => x.BookingID);
            // End One-To-Many relationship between Notification_Booking and Booking







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
