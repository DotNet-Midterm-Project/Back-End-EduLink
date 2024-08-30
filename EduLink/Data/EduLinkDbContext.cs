using EduLink.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Data
{
    public class EduLinkDbContext : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<WorkShop> WorkShops { get; set; }
        public DbSet<VolunteerCourse> VolunteerCourses { get; set; }
        public DbSet<EductionalContent> EductionalContents { get; set; }
        public DbSet<NotificationWorkshops> NotificationWorkshops { get; set; }

        public EduLinkDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the primary key for the Student entity
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentID);

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
        }
    }
}
