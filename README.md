# 🎓 EduAid Platform

**EduAid** is a volunteer-driven platform designed to support students at the College of Information Technology by offering academic sessions and personalized assistance through dedicated student volunteers. The system is built to streamline the academic support process by connecting students with volunteers, organizing academic events, and providing administrators with tools to manage and analyze academic activities.

## 📊 ERD Diagram

![EduAid Platform ERD] <!-- ؟؟؟؟؟؟؟؟؟؟؟؟؟ -->

## 🛠️ Overview of Relationships

The EduAid Platform is structured around three main entities: **Students**, **Student Volunteers**, and **Administrators**. These entities are interconnected through key relationships that define how the platform functions.

### 🧑‍🎓 Students
- **Courses**: Each student is enrolled in one or more courses, which are organized by their respective academic departments.
- **Sessions**: Students can book sessions with volunteers for specific courses. A session is tied to a student, a volunteer, and a specific course.
- **Evaluations**: After each session, students can evaluate the volunteer, which impacts the volunteer’s visibility in future searches.

### 🤝 Student Volunteers
- **Courses**: Volunteers can offer assistance for one or more courses based on their skills and expertise.
- **Sessions**: Volunteers manage student booking requests, accepting or declining session requests. Each session is tied to a course, a student, and a volunteer.
- **Events**: Volunteers can organize college-level academic events that students can participate in.

### 👨‍💼 Administrators
- **Department Management**: Administrators oversee academic departments and the courses offered within them.
- **Volunteer Management**: Administrators approve or reject volunteer requests based on qualifications and course needs.
- **Data Analysis**: Administrators have access to platform data to analyze student engagement, volunteer performance, and system efficiency.

## 🔗 Entity Relationships Summary
- **Student ↔️ Course**: A student can be enrolled in multiple courses, and each course can have multiple students.
- **Student ↔️ Session ↔️ Volunteer**: Students can book sessions with volunteers for specific courses. Each session connects a student, a volunteer, and a course.
- **Volunteer ↔️ Course**: A volunteer can assist in multiple courses, and each course can have multiple volunteers.
- **Administrator ↔️ Department/Course**: Administrators manage departments, courses, and volunteer applications.

## 🔧 Tech Stack
- **Frontend**: React, HTML, CSS
- **Backend**: .Net-Core, Entity-Framework
- **Database**: SQL Server
- **Authentication**: JWT
  
---

**Made with 💙 by the EduAid Team.**

