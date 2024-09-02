using EduLink.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users {  get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
       
        public DbSet<Department> Departments { get; set; }
        public DbSet<Event> Reservations { get; set; }
        public DbSet<VolunteerCourse> VolunteerCourses { get; set; }
        public DbSet<EductionalContent> EductionalContents { get; set; }
        public DbSet<DepartmentCourses> Department_courses { get; set; }
        public DbSet<Notification_Booking> NotificationBookings { get; set; }
        public DbSet<Anouncement> Anouncements { get; set; }
       

        public EduLinkDbContext(DbContextOptions options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the primary key for the Student entity
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentID);

            // Configure the one-to-one relationship between Volunteer and Student
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.Student)                  // Each Volunteer has one Student
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


            // Configure the primary key for the Course entity
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseID);

            // Configure the primary key for the EductionalContent entity
            modelBuilder.Entity<EductionalContent>()
                .HasKey(ec => ec.ContentID);

            // Configure the composite primary key for the VolunteerCourse entity
            modelBuilder.Entity<VolunteerCourse>()
                .HasKey(x=>x.VolunteerCourseID);


        
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


            // Configure the primary key for the NotificationWorkshops entity
            modelBuilder.Entity<Anouncement>()
                .HasKey(nw => nw.AnouncementID);

            // Configure the one-to-one relationship between WorkShop and NotificationWorkshops


            modelBuilder.Entity<User>().HasKey(u => u.Id);  // Set primary key for User entity
            
            modelBuilder.Entity<Student>().HasKey(u => u.StudentID); // Set primary key for Student entity
            modelBuilder.Entity<Department>().HasKey(u => u.DepartmentID); // Set primary key for Department entity
          
            modelBuilder.Entity<Course>().HasKey(x => x.CourseID);  // Set primary key for Course entity
            
          
            modelBuilder.Entity<Notification_Booking>().HasKey(x => x.NotificationID); // Set primary key for Notification_Booking entity 

            // Define One-To-One relationship between User and Student
            modelBuilder.Entity<Student>()
               .HasOne(x => x.User)
               .WithOne(x => x.Student)
               .HasForeignKey<Student>(x => x.UserID)
               .OnDelete(DeleteBehavior.NoAction); ;
            // End One-To-One relationship between User and Student

          

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
                .WithMany(x => x.DepartmentCourses)
                .HasForeignKey(x => x.CourseID);
            // End Many-To-Many relationship between Course and Department


         
            // Define One-To-Many relationship between Notification_Booking and Booking
            modelBuilder.Entity<Notification_Booking>()
                .HasOne(x => x.Booking)
                .WithMany(x => x.Notification_Bookings)
                .HasForeignKey(x => x.BookingID)
                .OnDelete(DeleteBehavior.NoAction);
            // End One-To-Many relationship between Notification_Booking and Booking

           

            //Relation between Event and Booking (one to one)
            modelBuilder.Entity<Event>()
                .HasMany(r => r.Bookings)   // One booking can be associated with multiple bookings
                .WithOne(b => b.Event) // One booking is associated with one reservation
                .HasForeignKey(b => b.EventID)  
                .OnDelete(DeleteBehavior.NoAction);

            // Relation between Student and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Student)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.StudentID);
            // Define One-To-Many relationship between EcucationContent and VolunteerCourse
            modelBuilder.Entity<EductionalContent>()
                .HasOne(e => e.VolunteerCourse)
                .WithMany(e => e.EductionalContent)
                .HasForeignKey(e => e.VolunteerCourseID);



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

