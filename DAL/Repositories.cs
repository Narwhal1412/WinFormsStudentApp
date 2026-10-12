using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Repository<T> where T : class
    {
        public List<T> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return db.Set<T>().AsNoTracking().ToList();
            }
        }

        public T GetById(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Set<T>().Find(id);
            }
        }

        public void Add(T item)
        {
            using (var db = new AppDbContext())
            {
                db.Set<T>().Add(item);
                db.SaveChanges();
            }
        }

        public void Update(T item)
        {
            using (var db = new AppDbContext())
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(T item)
        {
            using (var db = new AppDbContext())
            {
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }

    public class UserRepository
    {
        public bool CheckLogin(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                return db.Users.Any(u => u.Username == username && u.Password == password);
            }
        }
    }

    public class EnrollmentRepository
    {
        public List<Student> GetStudentsByCourse(int courseId)
        {
            using (var db = new AppDbContext())
            {
                var studentIds = db.StudentCourses
                    .Where(sc => sc.CourseID == courseId)
                    .Select(sc => sc.StudentID);
                return db.Students.Where(s => studentIds.Contains(s.StudentID)).ToList();
            }
        }

        public bool IsEnrolled(int studentId, int courseId)
        {
            using (var db = new AppDbContext())
            {
                return db.StudentCourses.Find(studentId, courseId) != null;
            }
        }

        public void Enroll(int studentId, int courseId)
        {
            using (var db = new AppDbContext())
            {
                db.StudentCourses.Add(new StudentCourse { StudentID = studentId, CourseID = courseId });
                db.SaveChanges();
            }
        }

        public void Move(int studentId, int fromCourseId, int toCourseId)
        {
            using (var db = new AppDbContext())
            {
                var row = db.StudentCourses.Find(studentId, fromCourseId);
                if (row != null)
                {
                    db.StudentCourses.Remove(row);
                }
                db.StudentCourses.Add(new StudentCourse { StudentID = studentId, CourseID = toCourseId });
                db.SaveChanges();
            }
        }

        public void Unenroll(int studentId, int courseId)
        {
            using (var db = new AppDbContext())
            {
                var row = db.StudentCourses.Find(studentId, courseId);
                if (row != null)
                {
                    db.StudentCourses.Remove(row);
                    db.SaveChanges();
                }
            }
        }
    }
}
