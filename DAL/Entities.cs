using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("tblUsers")]
    public class User
    {
        [Key, ReadOnly(true)]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Table("tblStudents")]
    public class Student
    {
        [Key, ReadOnly(true)]
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public int YearOfEnroll { get; set; }
        public string Major { get; set; }
        public decimal? GPA { get; set; }

        public Student()
        {
            DateOfBirth = DateTime.Today;
        }
    }

    [Table("tblCourses")]
    public class Course
    {
        [Key, ReadOnly(true)]
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public int DepartmentID { get; set; }
        public int InstructorID { get; set; }
    }

    [Table("tblInstructors")]
    public class Instructor
    {
        [Key, ReadOnly(true)]
        public int InstructorID { get; set; }
        public string InstructorName { get; set; }
        public int DepartmentID { get; set; }
    }

    [Table("tblDepartments")]
    public class Department
    {
        [Key, ReadOnly(true)]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }

    [Table("tblStudentCourses")]
    public class StudentCourse
    {
        [Key, Column(Order = 0)]
        public int StudentID { get; set; }
        [Key, Column(Order = 1)]
        public int CourseID { get; set; }
    }
}
