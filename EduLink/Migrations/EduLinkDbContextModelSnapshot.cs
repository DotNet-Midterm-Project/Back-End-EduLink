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

            modelBuilder.Entity("EduLink.Models.Announcement", b =>
                {
                    b.Property<int>("AnouncementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnouncementID"));

                    b.Property<DateTime>("AnounceDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int?>("SessionID")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("AnouncementID");

                    b.HasIndex("EventID");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("EduLink.Models.Article", b =>
                {
                    b.Property<int>("ArticleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleID"));

                    b.Property<string>("ArticleContent")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("ArticleFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

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

                    b.Property<int>("BookingStatus")
                        .HasColumnType("int");

                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.Property<int?>("SessionID")
                        .HasColumnType("int");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("EventID");

                    b.HasIndex("SessionID");

                    b.HasIndex("StudentID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("EduLink.Models.Comment", b =>
                {
                    b.Property<int>("CommentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentID"));

                    b.Property<int>("ArticleID")
                        .HasColumnType("int");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CommentID");

                    b.HasIndex("ArticleID");

                    b.HasIndex("UserID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("EduLink.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseID"));

                    b.Property<string>("CourseDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("CourseID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("EduLink.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EduLink.Models.DepartmentCourses", b =>
                {
                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.HasKey("CourseID", "DepartmentID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("DepartmentCourses");
                });

            modelBuilder.Entity("EduLink.Models.Event", b =>
                {
                    b.Property<int>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventID"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int?>("CourseID")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("EndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("EventAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventBannerImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("EventDetailes")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("EventStatus")
                        .HasColumnType("int");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<int?>("SessionCount")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("VolunteerCoursID")
                        .HasColumnType("int");

                    b.Property<int?>("VolunteerID")
                        .HasColumnType("int");

                    b.HasKey("EventID");

                    b.HasIndex("CourseID");

                    b.HasIndex("VolunteerCoursID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EduLink.Models.EventContent", b =>
                {
                    b.Property<int>("ContentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentID"));

                    b.Property<string>("ContentAddress")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("ContentDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ContentName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("ContentType")
                        .HasColumnType("int");

                    b.Property<string>("EventContentFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.HasKey("ContentID");

                    b.HasIndex("EventID");

                    b.ToTable("EventContents");
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
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("BookingID")
                        .IsUnique();

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("EduLink.Models.Like", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ArticleID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "ArticleID");

                    b.HasIndex("ArticleID");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("EduLink.Models.Session", b =>
                {
                    b.Property<int>("SessionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SessionID"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasMaxLength(100)
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.Property<int>("SessionStatus")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasMaxLength(100)
                        .HasColumnType("datetimeoffset");

                    b.HasKey("SessionID");

                    b.HasIndex("EventID");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("EduLink.Models.Student", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentID"));

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StudentID");

                    b.HasIndex("UserID")
                        .IsUnique();

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

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
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

                    b.Property<string>("ProfileImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("RefreshTokenExpireTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Skills")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("TempCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset>("TempCodeExpire")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentID");

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

                    b.Property<bool>("Approve")
                        .HasColumnType("bit");

                    b.Property<int>("Availability")
                        .HasColumnType("int");

                    b.Property<float?>("Rating")
                        .HasColumnType("real");

                    b.Property<int?>("RatingAcount")
                        .HasColumnType("int");

                    b.Property<string>("SkillDescription")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("VolunteerID");

                    b.HasIndex("StudentID")
                        .IsUnique();

                    b.ToTable("Volunteers");
                });

            modelBuilder.Entity("EduLink.Models.VolunteerCourse", b =>
                {
                    b.Property<int>("VolunteerCourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VolunteerCourseID"));

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("int");

                    b.HasKey("VolunteerCourseID");

                    b.HasIndex("CourseID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("VolunteerCourses");
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

                    b.HasData(
                        new
                        {
                            Id = "admin",
                            ConcurrencyStamp = "00000000-0000-0000-0000-000000000000",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "student",
                            ConcurrencyStamp = "00000000-0000-0000-0000-000000000000",
                            Name = "Student",
                            NormalizedName = "STUDENT"
                        },
                        new
                        {
                            Id = "volunteer",
                            ConcurrencyStamp = "00000000-0000-0000-0000-000000000000",
                            Name = "Volunteer",
                            NormalizedName = "VOLUNTEER"
                        });
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

            modelBuilder.Entity("EduLink.Models.Announcement", b =>
                {
                    b.HasOne("EduLink.Models.Event", "Event")
                        .WithMany("Announcements")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("EduLink.Models.Article", b =>
                {
                    b.HasOne("EduLink.Models.Volunteer", "Volunteer")
                        .WithMany("Articles")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("EduLink.Models.Booking", b =>
                {
                    b.HasOne("EduLink.Models.Event", "Event")
                        .WithMany("Bookings")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Session", "Session")
                        .WithMany("Bookings")
                        .HasForeignKey("SessionID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Student", "Student")
                        .WithMany("Bookings")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Session");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EduLink.Models.Comment", b =>
                {
                    b.HasOne("EduLink.Models.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleID")
                        .IsRequired();

                    b.HasOne("EduLink.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EduLink.Models.DepartmentCourses", b =>
                {
                    b.HasOne("EduLink.Models.Course", "Course")
                        .WithMany("DepartmentCourses")
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

            modelBuilder.Entity("EduLink.Models.Event", b =>
                {
                    b.HasOne("EduLink.Models.Course", null)
                        .WithMany("Events")
                        .HasForeignKey("CourseID");

                    b.HasOne("EduLink.Models.VolunteerCourse", "VolunteerCourse")
                        .WithMany("Events")
                        .HasForeignKey("VolunteerCoursID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Volunteer", null)
                        .WithMany("Reservations")
                        .HasForeignKey("VolunteerID");

                    b.Navigation("VolunteerCourse");
                });

            modelBuilder.Entity("EduLink.Models.EventContent", b =>
                {
                    b.HasOne("EduLink.Models.Event", "Event")
                        .WithMany("EventContents")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("EduLink.Models.Feedback", b =>
                {
                    b.HasOne("EduLink.Models.Booking", "Booking")
                        .WithOne("Feedbacks")
                        .HasForeignKey("EduLink.Models.Feedback", "BookingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("EduLink.Models.Like", b =>
                {
                    b.HasOne("EduLink.Models.Article", "Article")
                        .WithMany("Likes")
                        .HasForeignKey("ArticleID")
                        .IsRequired();

                    b.HasOne("EduLink.Models.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserID")
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EduLink.Models.Session", b =>
                {
                    b.HasOne("EduLink.Models.Event", "Event")
                        .WithMany("Sessions")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("EduLink.Models.Student", b =>
                {
                    b.HasOne("EduLink.Models.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("EduLink.Models.Student", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EduLink.Models.User", b =>
                {
                    b.HasOne("EduLink.Models.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("EduLink.Models.Volunteer", b =>
                {
                    b.HasOne("EduLink.Models.Student", "Student")
                        .WithOne("Volunteer")
                        .HasForeignKey("EduLink.Models.Volunteer", "StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EduLink.Models.VolunteerCourse", b =>
                {
                    b.HasOne("EduLink.Models.Course", "Course")
                        .WithMany("volunteerCourses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduLink.Models.Volunteer", "Volunteer")
                        .WithMany("VolunteerCourse")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Volunteer");
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

            modelBuilder.Entity("EduLink.Models.Article", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("EduLink.Models.Booking", b =>
                {
                    b.Navigation("Feedbacks")
                        .IsRequired();
                });

            modelBuilder.Entity("EduLink.Models.Course", b =>
                {
                    b.Navigation("DepartmentCourses");

                    b.Navigation("Events");

                    b.Navigation("volunteerCourses");
                });

            modelBuilder.Entity("EduLink.Models.Department", b =>
                {
                    b.Navigation("Department_Courses");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("EduLink.Models.Event", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Bookings");

                    b.Navigation("EventContents");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("EduLink.Models.Session", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("EduLink.Models.Student", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Volunteer")
                        .IsRequired();
                });

            modelBuilder.Entity("EduLink.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Student")
                        .IsRequired();
                });

            modelBuilder.Entity("EduLink.Models.Volunteer", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Reservations");

                    b.Navigation("VolunteerCourse");
                });

            modelBuilder.Entity("EduLink.Models.VolunteerCourse", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
