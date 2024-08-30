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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring relationships between all Entities.



           
            modelBuilder.Entity<User>().HasKey(u => u.Id);  // Set primary key for User entity
            
            modelBuilder.Entity<Student>().HasKey(u => u.StudentID); // Set primary key for Student entity
            modelBuilder.Entity<Department>().HasKey(u => u.DepartmentID); // Set primary key for Department entity
            modelBuilder.Entity<Department_Courses>().HasKey(x => new { x.CourseID, x.DepartmentID }); // Set composite key for Department_Courses entity
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
               .WithMany(x => x.students)
               .HasForeignKey(x => x.DepartmentID)
               .OnDelete(DeleteBehavior.SetNull);
            // End One-To-Many relationship between Department and Student

            // Define Many-To-Many relationship between Department and Courses
            modelBuilder.Entity<Department_Courses>()
                .HasOne(x => x.department)
                .WithMany(x => x.department_Courses)
                .HasForeignKey(x => x.DepartmentID);
                
            

           
            modelBuilder.Entity<Department_Courses>()
                .HasOne(x => x.course)
                .WithMany(x => x.department_Courses)
                .HasForeignKey(x => x.CourseID);
            // End Many-To-Many relationship between Course and Department


            // Define One-To-Many relationship between Notification_Booking and Student
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.Student)
                .WithMany(x => x.notification_Bookings)
                .HasForeignKey(x => x.StudentID);
            // End One-To-Many relationship etween Notification_Booking and Student

            // Define One-To-Many relationship between Notification_Booking and Booking
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.booking)
                .WithMany(x => x.notification_Bookings)
                .HasForeignKey(x => x.BookingID);
            // End One-To-Many relationship between Notification_Booking and Booking






        }
    }
}
