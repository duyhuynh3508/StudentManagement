using ProductManagement.Helpers;
using StudentManagement.Data;
using StudentManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll();
        Student GetById(int id);
        Student Add(Student _std);
        Student Update(Student _std);
        int totalStudent();
        int numberPage(int totalStudent, int limit);
        IEnumerable<Student> paginationStudent(int start, int limit);
        void Delete(int id);
    }
    public class StudentService : IStudentService
    {
        public StudentDBContext _db;
        public StudentService(StudentDBContext db)
        {
            _db = db;
        }
        public Student Add(Student _std)
        {
            if (string.IsNullOrWhiteSpace(_std.StudentName))
                throw new AppException("Name is required");
            if (string.IsNullOrWhiteSpace(_std.Address))
                throw new AppException("Address is required");
            if (string.IsNullOrWhiteSpace(_std.PhoneNumber))
                throw new AppException("PhoneNumber is required");
            var obj = new Student
            {
                StudentName = _std.StudentName,
                Address = _std.Address,
                Class = _db.Class.Find(_std.ClassID),
                PhoneNumber = _std.PhoneNumber
            };
            _db.Student.Add(obj);
            _db.SaveChanges();
            return obj;

        }

        public void Delete(int id)
        {
            var obj = _db.Student.Find(id);
            _db.Student.Remove(obj);
        }

        public IEnumerable<Student> GetAll()
        {
            var std = _db.Student.Select(s => new Student
            {
                StudentID = s.StudentID,
                StudentName = s.StudentName,
                Address = s.Address,
                PhoneNumber = s.PhoneNumber,
                Class = _db.Class.Where(a => a.ClassName == s.Class.ClassName).FirstOrDefault()
            }).ToList();
            return std;
        }

        public Student GetById(int id)
        {
            var obj = _db.Student.Find(id);
            return obj;
        }

        public int numberPage(int totalStudent, int maxRow)
        {
            float numberpage = 0;
            if(totalStudent % maxRow == 0)
            {
                numberpage = totalStudent % maxRow;
            }
            else
            {
                numberpage = (totalStudent / maxRow) + 1;
            }
            return (int)Math.Ceiling(numberpage);

        }

        public IEnumerable<Student> paginationStudent(int currenPage, int maxRow)
        {
            var data = _db.Student;
            var dataStudent = data.OrderByDescending(x => x.StudentID).Skip((currenPage-1)*maxRow).Take(maxRow);
            return dataStudent.ToList();

        }

        public int totalStudent()
        {
            return _db.Student.Count();
        }

        public Student Update(Student _std)
        {
            var obj = _db.Student.Find(_std.StudentID);
            if (obj == null)
                throw new AppException("Studen is not found");
            if (!string.IsNullOrWhiteSpace(_std.StudentName))
                 obj.StudentName = _std.StudentName;
            if (!string.IsNullOrWhiteSpace(_std.Address))
                obj.Address = _std.Address;
            if (!string.IsNullOrWhiteSpace(_std.PhoneNumber))
                obj.PhoneNumber = _std.PhoneNumber;
            obj.Class = _db.Class.Find(_std.ClassID);
            _db.Student.Update(obj);
            _db.SaveChanges();
            return obj;
        }
    }
}
