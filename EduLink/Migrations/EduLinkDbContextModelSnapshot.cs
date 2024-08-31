﻿// <auto-generated />
using System;
using EduLink.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EduLink.Migrations
{
    [DbContext(typeof(EduLinkDbContext))]
    partial class EduLinkDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EduLink.Models.Admin", b =>
                {
                    b.Property<string>("AdminID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AdminID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("EduLink.Models.Article", b =>
                {
                    b.Property<int>("ArticleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleID"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("int");

                    b.HasKey("ArticleID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("EduLink.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"));

                    b.Property<int>("ReservationID")
                        .HasColumnType("int");

                    b.Property<string>("SessionLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SessionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BookingID");

                    b.HasIndex("ReservationID");

                    b.HasIndex("StudentID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("EduLink.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseID"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseID");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("EduLink.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EduLink.Models.Department_Courses", b =>
                {
                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.HasKey("CourseID", "DepartmentID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Department_courses");
                });

            modelBuilder.Entity("EduLink.Models.EductionalContent", b =>
                {
                    b.Property<int>("ContentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentID"));

                    b.Property<string>("ContentDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("int");

                    b.HasKey("ContentID");

                    b.HasIndex("CourseID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("EductionalContents");
                });

            modelBuilder.Entity("EduLink.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackID"));

                    b.Property<int>("BookingID")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("BookingID")
                        .IsUnique();

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("EduLink.Models.NotificationWorkshops", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateSend")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkshopID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("WorkshopID")
                        .IsUnique();

                    b.ToTable("NotificationWorkshops");
                });

            modelBuilder.Entity("EduLink.Models.Notification_Booking", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"));

                    b.Property<int>("BookingID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NotificationID");

                    b.HasIndex("BookingID");

                    b.HasIndex("StudentID");

                    b.ToTable("NotificationBookings");
                });

            modelBuilder.Entity("EduLink.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationID"));

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("int");

                    b.HasKey("ReservationID");

                    b.HasIndex("CourseID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("EduLink.Models.Student", b =>
                {
                    b.Property<string>("StudentID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.HasKey("StudentID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("EduLink.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("EduLink.Models.Volunteer", b =>
                {
                    b.Property<int>("VolunteerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VolunteerID"));

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVolunteer")
                        .HasColumnType("bit");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("SkillDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("VolunteerID");

                    b.HasIndex("StudentID")
                        .IsUnique();

                    b.ToTable("Volunteers");
                });

            modelBuilder.Entity("EduLink.Models.VolunteerCourse", b =>
                {
                    b.Property<int>("VolunteerID")
                        .HasColumnType("int");

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.HasKey("VolunteerID", "CourseID");

                    b.HasIndex("CourseID");

                    b.ToTable("VolunteerCourses");
                });

            modelBuilder.Entity("EduLink.Models.WorkShop", b =>
                {
                    b.Property<int>("WorkShopID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkShopID"));

                    b.Property<int>("Capasity")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SessionLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("int");

                    b.HasKey("WorkShopID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("WorkShops");
                });

            modelBuilder.Entity("EduLink.Models.WorkshopsRegistration", b =>
                {
                    b.Property<int>("WorkShopID")
                        .HasColumnType("int");

                    b.Property<string>("StudentID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("WorkShopID", "StudentID");

                    b.HasIndex("StudentID");

                    b.ToTable("WorkshopsRegistration");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EduLink.Models.Admin", b =>
                {
                    b.HasOne("EduLink.Models.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("EduLink.Models.Admin", "AdminID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EduLink.Models.Article", b =>
                {
                    b.HasOne("EduLink.Models.Volunteer", "Volunteer")
                        .WithMany("Articles")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("EduLink.Models.Booking", b =>
                {
                    b.HasOne("EduLink.Models.Reservation", "Reservation")
                        .WithMany("Bookings")
                        .HasForeignKey("ReservationID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Student", "Student")
                        .WithMany("Bookings")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EduLink.Models.Department_Courses", b =>
                {
                    b.HasOne("EduLink.Models.Course", "Course")
                        .WithMany("Department_Courses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Department", "Department")
                        .WithMany("Department_Courses")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("EduLink.Models.EductionalContent", b =>
                {
                    b.HasOne("EduLink.Models.Course", "Course")
                        .WithMany("EductionalContents")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Volunteer", "Volunteers")
                        .WithMany("EductionalContent")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Volunteers");
                });

            modelBuilder.Entity("EduLink.Models.Feedback", b =>
                {
                    b.HasOne("EduLink.Models.Booking", "Booking")
                        .WithOne("Feedbacks")
                        .HasForeignKey("EduLink.Models.Feedback", "BookingID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("EduLink.Models.NotificationWorkshops", b =>
                {
                    b.HasOne("EduLink.Models.WorkShop", "WorkShop")
                        .WithOne("NotificationWorkshops")
                        .HasForeignKey("EduLink.Models.NotificationWorkshops", "WorkshopID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("WorkShop");
                });

            modelBuilder.Entity("EduLink.Models.Notification_Booking", b =>
                {
                    b.HasOne("EduLink.Models.Booking", "Booking")
                        .WithMany("Notification_Bookings")
                        .HasForeignKey("BookingID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Student", "Student")
                        .WithMany("Notification_Bookings")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EduLink.Models.Reservation", b =>
                {
                    b.HasOne("EduLink.Models.Course", "Course")
                        .WithMany("Reservations")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Volunteer", "Volunteer")
                        .WithMany("Reservations")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("EduLink.Models.Student", b =>
                {
                    b.HasOne("EduLink.Models.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("EduLink.Models.Student", "StudentID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EduLink.Models.Volunteer", b =>
                {
                    b.HasOne("EduLink.Models.Student", "Students")
                        .WithOne("Volunteers")
                        .HasForeignKey("EduLink.Models.Volunteer", "StudentID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Students");
                });

            modelBuilder.Entity("EduLink.Models.VolunteerCourse", b =>
                {
                    b.HasOne("EduLink.Models.Course", "Course")
                        .WithMany("volunteerCourses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Volunteer", "Volunteers")
                        .WithMany("VolunteerCourse")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Volunteers");
                });

            modelBuilder.Entity("EduLink.Models.WorkShop", b =>
                {
                    b.HasOne("EduLink.Models.Volunteer", "Volunteer")
                        .WithMany("WorkShops")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("EduLink.Models.WorkshopsRegistration", b =>
                {
                    b.HasOne("EduLink.Models.Student", "Student")
                        .WithMany("WorkshopsRegistrations")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.WorkShop", "WorkShop")
                        .WithMany("WorkshopsRegistrations")
                        .HasForeignKey("WorkShopID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("WorkShop");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EduLink.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EduLink.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EduLink.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduLink.Models.Booking", b =>
                {
                    b.Navigation("Feedbacks")
                        .IsRequired();

                    b.Navigation("Notification_Bookings");
                });

            modelBuilder.Entity("EduLink.Models.Course", b =>
                {
                    b.Navigation("Department_Courses");

                    b.Navigation("EductionalContents");

                    b.Navigation("Reservations");

                    b.Navigation("volunteerCourses");
                });

            modelBuilder.Entity("EduLink.Models.Department", b =>
                {
                    b.Navigation("Department_Courses");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("EduLink.Models.Reservation", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("EduLink.Models.Student", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Notification_Bookings");

                    b.Navigation("Volunteers")
                        .IsRequired();

                    b.Navigation("WorkshopsRegistrations");
                });

            modelBuilder.Entity("EduLink.Models.User", b =>
                {
                    b.Navigation("Admin")
                        .IsRequired();

                    b.Navigation("Student")
                        .IsRequired();
                });

            modelBuilder.Entity("EduLink.Models.Volunteer", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("EductionalContent");

                    b.Navigation("Reservations");

                    b.Navigation("VolunteerCourse");

                    b.Navigation("WorkShops");
                });

            modelBuilder.Entity("EduLink.Models.WorkShop", b =>
                {
                    b.Navigation("NotificationWorkshops")
                        .IsRequired();

                    b.Navigation("WorkshopsRegistrations");
                });
#pragma warning restore 612, 618
        }
    }
}
