using EduLink.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users {  get; set; }
         public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<VolunteerCourse> VolunteerCourses { get; set; }
        public DbSet<EventContent> EventContents { get; set; }
        public DbSet<DepartmentCourses> DepartmentCourses { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<Session> Sessions { get; set; }


        public EduLinkDbContext(DbContextOptions options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<User>()
           .Property(u => u.TempCode)
           .IsRequired(false);
    

            modelBuilder.Entity<Volunteer>()
             .Property(v => v.Rating)
            .IsRequired(false);
            modelBuilder.Entity<Volunteer>()
          .Property(v => v.RatingAcount)
         .IsRequired(false);
          
            modelBuilder.Entity<Booking>()
         .Property(b => b.SessionID)
         .IsRequired(false);
          


            // Configure the primary key for the Student entity
            modelBuilder.Entity<Volunteer>()
                .HasKey(v => v.VolunteerID);
            // Configure the primary key for the Article entity
            modelBuilder.Entity<Article>()
                .HasKey(a => a.ArticleID);

            // Configure the primary key for the Course entity
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseID);

            // Configure the primary key for the EductionalContent entity
            modelBuilder.Entity<EventContent>()
                .HasKey(ec => ec.ContentID);

            // Configure the composite primary key for the VolunteerCourse entity
            modelBuilder.Entity<VolunteerCourse>()
                .HasKey(x => x.VolunteerCourseID);

            // Configure the primary key for the Booking entity
            modelBuilder.Entity<Booking>()
                .HasKey(b => b.BookingID);
            // Configure the primary key for the Feedback entity
            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.FeedbackID);
            // Configure the primary key for the NotificationWorkshops entity
            modelBuilder.Entity<Announcement>()
                .HasKey(nw => nw.AnouncementID);

            // Configure the one-to-one relationship between WorkShop and NotificationWorkshops


            modelBuilder.Entity<User>().HasKey(u => u.Id);  // Set primary key for Student entity

         
            modelBuilder.Entity<Department>().HasKey(u => u.DepartmentID); // Set primary key for Department entity

            modelBuilder.Entity<Course>().HasKey(x => x.CourseID);  // Set primary key for Course entity

            // Configure the one-to-one relationship between Student and User
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)                  // Each Student has one Student
                .WithOne(u => u.Student)               // Each Student has one Student
                .HasForeignKey<Student>(s => s.UserID) // Foreign key in Student entity
                .OnDelete(DeleteBehavior.Cascade);



            // Configure the one-to-one relationship between Student and Volunteer
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.Student)                  // Each Student has one Student
                .WithOne(s => s.Volunteer)               // Each Student has one Student
                .HasForeignKey<Volunteer>(v => v.StudentID) // Foreign key in Student entity
                .OnDelete(DeleteBehavior.Cascade);

        
            // Configure the many-to-many relationship between Student and Course via VolunteerCourse
            modelBuilder.Entity<VolunteerCourse>()
                .HasOne(vc => vc.Volunteer)               // Each VolunteerCourse has one Student
                .WithMany(v => v.VolunteerCourse)          // Each Student has many VolunteerCourses
                .HasForeignKey(vc => vc.VolunteerID)      // Foreign key in VolunteerCourse entity
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VolunteerCourse>()
                .HasOne(vc => vc.Course)                  // Each VolunteerCourse has one Course
                .WithMany(c => c.volunteerCourses)         // Each Course has many VolunteerCourses
                .HasForeignKey(vc => vc.CourseID)         // Foreign key in VolunteerCourse entity
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the one-to-many relationship between Volunteer and Article
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Volunteer)           // Each Article has one Student
                .WithMany(v => v.Articles)          // Each Student has many Articles
                .HasForeignKey(a => a.VolunteerID)  // Foreign key in Article entity
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between VolunteerCourse and Event (one-to-many)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.VolunteerCourse)
                .WithMany(vc => vc.Events)
                .HasForeignKey(e => e.VolunteerCoursID)
                .OnDelete(DeleteBehavior.Cascade);  // Use Cascade or SetNull depending on your needs


        

            // Configure the one-to-one relationship between Booking and Feedback
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Feedbacks)                  // Each Booking has one Feedback
                .WithOne(f => f.Booking)                   // Each Feedback has one Booking
                .HasForeignKey<Feedback>(f => f.BookingID) // Foreign key in Feedback entity
                .OnDelete(DeleteBehavior.Cascade);
       
  

          

            // Define One-To-Many relationship between Department and User
             modelBuilder.Entity<User>()
               .HasOne(x => x.Department)
               .WithMany(x => x.Users)
               .HasForeignKey(x => x.DepartmentID)
               .OnDelete(DeleteBehavior.Cascade);
            // End One-To-Many relationship between Department and User

            // Define Many-To-Many relationship between Department and Course
            modelBuilder.Entity<DepartmentCourses>().HasKey(x => new { x.CourseID, x.DepartmentID }); // Set composite key for Department_Courses entity
            modelBuilder.Entity<DepartmentCourses>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Department_Courses)
                .HasForeignKey(x => x.DepartmentID).OnDelete(DeleteBehavior.NoAction); 
                
            modelBuilder.Entity<DepartmentCourses>()
                .HasOne(x => x.Course)
                .WithMany(x => x.DepartmentCourses)
                .HasForeignKey(x => x.CourseID).OnDelete(DeleteBehavior.NoAction);
   

            // Relationship between Event and Announcement (one-to-many)
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Announcements)
                .HasForeignKey(a => a.EventID)
                .OnDelete(DeleteBehavior.NoAction);  
            // Use Cascade or Restrict depending on your needs


            //Relation between Event and Booking (one to many)
            modelBuilder.Entity<Event>()
                .HasMany(r => r.Bookings)   // One booking can be associated with multiple bookings
                .WithOne(b => b.Event) // One booking is associated with one Event
                .HasForeignKey(b => b.EventID)  
                .OnDelete(DeleteBehavior.Cascade);

            // Relation between Student and Booking(one to many)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Student)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.StudentID)
                .OnDelete(DeleteBehavior.Cascade);
            // Define One-To-Many relationship between EventContent and Event
            modelBuilder.Entity<EventContent>()
                .HasOne(e => e.Event)
                .WithMany(e => e.EventContents)
                .HasForeignKey(e => e.EventID)
                .OnDelete(DeleteBehavior.Cascade);
            // Define One-To-Many relationship between ُEvent and Session
            modelBuilder.Entity<Event>()
             .HasMany(e => e.Sessions)
             .WithOne(s => s.Event)
             .HasForeignKey(s => s.EventID)
             .OnDelete(DeleteBehavior.Cascade);
            // Define One-To-Many relationship between Booking and Session
            modelBuilder.Entity<Session>()
                           .HasMany(s => s.Bookings)
                           .WithOne(b => b.Session)
                           .HasForeignKey(b => b.SessionID)
                           .OnDelete(DeleteBehavior.NoAction);


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

