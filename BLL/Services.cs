using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Service<T> where T : class
    {
        private readonly Repository<T> _repository = new Repository<T>();

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T Get(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(T item)
        {
            Validate(item);
            _repository.Add(item);
        }

        public void Update(T item)
        {
            Validate(item);
            _repository.Update(item);
        }

        public void Delete(T item)
        {
            _repository.Delete(item);
        }

        protected virtual void Validate(T item)
        {
        }

        protected static void Require(bool condition, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }
    }

    public class UserService
    {
        private readonly UserRepository _repository = new UserRepository();

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            return _repository.CheckLogin(username, password);
        }
    }

    public class StudentService : Service<Student>
    {
        protected override void Validate(Student s)
        {
            Require(!string.IsNullOrWhiteSpace(s.StudentName), "Student name is required.");
            Require(!string.IsNullOrWhiteSpace(s.City), "City is required.");
            Require(s.Age > 0, "Age must be greater than 0.");
            Require(!(s.GPA < 0 || s.GPA > 4), "GPA must be between 0 and 4.");
        }
    }

    public class CourseService : Service<Course>
    {
        protected override void Validate(Course c)
        {
            Require(!string.IsNullOrWhiteSpace(c.CourseName), "Course name is required.");
            Require(c.Credits > 0, "Credits must be greater than 0.");
        }
    }

    public class InstructorService : Service<Instructor>
    {
        protected override void Validate(Instructor i)
        {
            Require(!string.IsNullOrWhiteSpace(i.InstructorName), "Instructor name is required.");
        }
    }

    public class DepartmentService : Service<Department>
    {
        protected override void Validate(Department d)
        {
            Require(!string.IsNullOrWhiteSpace(d.DepartmentName), "Department name is required.");
        }
    }

    public class EnrollmentService
    {
        private readonly EnrollmentRepository _repository = new EnrollmentRepository();

        public List<Student> GetStudentsByCourse(int courseId)
        {
            return _repository.GetStudentsByCourse(courseId);
        }

        public void Enroll(int studentId, int courseId)
        {
            if (_repository.IsEnrolled(studentId, courseId))
            {
                throw new InvalidOperationException("Student is already enrolled in this course.");
            }
            _repository.Enroll(studentId, courseId);
        }

        public void Move(int studentId, int fromCourseId, int toCourseId)
        {
            if (fromCourseId == toCourseId)
            {
                throw new InvalidOperationException("Choose a different course to move to.");
            }
            if (_repository.IsEnrolled(studentId, toCourseId))
            {
                throw new InvalidOperationException("Student is already enrolled in the target course.");
            }
            _repository.Move(studentId, fromCourseId, toCourseId);
        }

        public void Unenroll(int studentId, int courseId)
        {
            _repository.Unenroll(studentId, courseId);
        }
    }
}
