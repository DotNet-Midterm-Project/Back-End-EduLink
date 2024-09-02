using EduLink.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<WorkShop> WorkShops { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<VolunteerCourse> VolunteerCourses { get; set; }
        public DbSet<EductionalContent> EductionalContents { get; set; }
        public DbSet<DepartmentCourses> Department_courses { get; set; }
        public DbSet<Notification_Booking> NotificationBookings { get; set; }
        public DbSet<NotificationWorkshops> NotificationWorkshops { get; set; }
        public DbSet<WorkshopsRegistration> WorkshopsRegistration { get; set; }

        public EduLinkDbContext(DbContextOptions options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the primary key for the Student entity
            modelBuilder.Entity<Student>()
                .HasKey(s => s.UserID);

            // Configure the one-to-one relationship between Volunteer and Student
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.Students)                  // Each Volunteer has one Student
                .WithOne(s => s.Volunteers)               // Each Student has one Volunteer
                .HasForeignKey<Volunteer>(v => v.StudentID) // Foreign key in Volunteer entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the primary key for the Volunteer entity
            modelBuilder.Entity<Volunteer>()
                .HasKey(v => v.VolunteerID);

            // Configure the primary key for the Article entity
            modelBuilder.Entity<Article>()
                .HasKey(a => a.ArticleID);

            // Configure the one-to-many relationship between Volunteer and Article
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Volunteer)           // Each Article has one Volunteer
                .WithMany(v => v.Articles)          // Each Volunteer has many Articles
                .HasForeignKey(a => a.VolunteerID)  // Foreign key in Article entity
                .OnDelete(DeleteBehavior.NoAction);
            // Configure the one-to-many relationship between Volunteer and EductionalContent
            modelBuilder.Entity<EductionalContent>()
                .HasOne(ec => ec.Volunteers)        // Each EductionalContent has one Volunteer
                .WithMany(v => v.EductionalContent) // Each Volunteer has many EductionalContents
                .HasForeignKey(ec => ec.VolunteerID) // Foreign key in EductionalContent entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the primary key for the Course entity
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseID);

            // Configure the primary key for the EductionalContent entity
            modelBuilder.Entity<EductionalContent>()
                .HasKey(ec => ec.ContentID);

            // Configure the composite primary key for the VolunteerCourse entity
            modelBuilder.Entity<VolunteerCourse>()
                .HasKey(vc => new { vc.VolunteerID, vc.CourseID });

            // Configure the many-to-one relationship between EductionalContent and Course
            modelBuilder.Entity<EductionalContent>()
                .HasOne(ec => ec.Courses)                  // Each EductionalContent is associated with one Course
                .WithMany(c => c.EductionalContents)       // Each Course has many EductionalContents
                .HasForeignKey(ec => ec.CourseID)          // Foreign key in EductionalContent entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the one-to-many relationship between Volunteer and EductionalContent
            modelBuilder.Entity<EductionalContent>()
                .HasOne(ec => ec.Volunteers)               // Each EductionalContent is associated with one Volunteer
                .WithMany(v => v.EductionalContent)        // Each Volunteer has many EductionalContents
                .HasForeignKey(ec => ec.VolunteerID)       // Foreign key in EductionalContent entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the many-to-many relationship between Volunteer and Course via VolunteerCourse
            modelBuilder.Entity<VolunteerCourse>()
                .HasOne(vc => vc.Volunteers)               // Each VolunteerCourse has one Volunteer
                .WithMany(v => v.VolunteerCourse)          // Each Volunteer has many VolunteerCourses
                .HasForeignKey(vc => vc.VolunteerID)      // Foreign key in VolunteerCourse entity
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VolunteerCourse>()
                .HasOne(vc => vc.Courses)                  // Each VolunteerCourse has one Course
                .WithMany(c => c.volunteerCourses)         // Each Course has many VolunteerCourses
                .HasForeignKey(vc => vc.CourseID)         // Foreign key in VolunteerCourse entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the primary key for the Booking entity
            modelBuilder.Entity<Booking>()
                .HasKey(b => b.BookingID);

            // Configure the primary key for the Feedback entity
            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.FeedbackID);

            // Configure the one-to-one relationship between Booking and Feedback
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Feedbacks)                  // Each Booking has one Feedback
                .WithOne(f => f.Booking)                   // Each Feedback has one Booking
                .HasForeignKey<Feedback>(f => f.BookingID) // Foreign key in Feedback entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the primary key for the WorkShop entity
            modelBuilder.Entity<WorkShop>()
                .HasKey(w => w.WorkShopID);

            // Configure the primary key for the NotificationWorkshops entity
            modelBuilder.Entity<NotificationWorkshops>()
                .HasKey(nw => nw.ID);

            // Configure the one-to-one relationship between WorkShop and NotificationWorkshops
            modelBuilder.Entity<WorkShop>()
                .HasOne(w => w.NotificationWorkshops)                  // Each WorkShop has one NotificationWorkshops
                .WithOne(nw => nw.WorkShop)                            // Each NotificationWorkshops has one WorkShop
                .HasForeignKey<NotificationWorkshops>(nw => nw.WorkshopID) // Foreign key in NotificationWorkshops entity
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().HasKey(u => u.Id);  // Set primary key for User entity
            
            modelBuilder.Entity<Student>().HasKey(u => u.UserID); // Set primary key for Student entity
            modelBuilder.Entity<Department>().HasKey(u => u.DepartmentID); // Set primary key for Department entity
          
            modelBuilder.Entity<Course>().HasKey(x => x.CourseID);  // Set primary key for Course entity
            modelBuilder.Entity<Admin>().HasKey(x => x.AdminID); // Set primary key for Admin entity
          
            modelBuilder.Entity<Notification_Booking>().HasKey(x => x.NotificationID); // Set primary key for Notification_Booking entity 

            // Define One-To-One relationship between User and Student
            modelBuilder.Entity<Student>()
               .HasOne(x => x.User)
               .WithOne(x => x.Student)
               .HasForeignKey<Student>(x => x.UserID)
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
               .HasForeignKey(x => x.DepartmentID);
            // End One-To-Many relationship between Department and Student

            // Define Many-To-Many relationship between Department and Course
            modelBuilder.Entity<DepartmentCourses>().HasKey(x => new { x.CourseID, x.DepartmentID }); // Set composite key for Department_Courses entity
            modelBuilder.Entity<DepartmentCourses>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Department_Courses)
                .HasForeignKey(x => x.DepartmentID);
                
            modelBuilder.Entity<DepartmentCourses>()
                .HasOne(x => x.Course)
                .WithMany(x => x.Department_Courses)
                .HasForeignKey(x => x.CourseID);
            // End Many-To-Many relationship between Course and Department


            // Define One-To-Many relationship between Notification_Booking and Student
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.Student)
                .WithMany(x => x.Notification_Bookings)
                .HasForeignKey(x => x.StudentID)
                .OnDelete(DeleteBehavior.NoAction);
            // End One-To-Many relationship etween Notification_Booking and Student

            // Define One-To-Many relationship between Notification_Booking and Booking
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.Booking)
                .WithMany(x => x.Notification_Bookings)
                .HasForeignKey(x => x.BookingID)
                .OnDelete(DeleteBehavior.NoAction);
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

            //Seed Roles
            SeedRoles(modelBuilder, "Admin");
            SeedRoles(modelBuilder, "Student");
            SeedRoles(modelBuilder, "Volunteer");
        }
        private void SeedRoles(ModelBuilder modelBuilder, string roleName, params string[] permission)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };

            // add claims for the users
            // complete


            modelBuilder.Entity<IdentityRole>().HasData(role);
        }

    }
}

