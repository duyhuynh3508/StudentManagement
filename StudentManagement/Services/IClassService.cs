using ProductManagement.Helpers;
using StudentManagement.Data;
using StudentManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IClassService
    {
        IEnumerable<Class> GetAll();
        Class GetByID(int id);
        Class Add(Class _class);
        Class Update(Class _class);
        void Delete(int id);
    }
    public class ClassService : IClassService
    {
        public StudentDBContext _db;
        public ClassService(StudentDBContext db)
        {
            _db = db;
        }
        public Class Add(Class _class)
        {
             if(_db.Class.Any(x => x.ClassName == _class.ClassName))
                 throw new AppException("Class name is taken");
            _db.Class.Add(_class);
            _db.SaveChanges();
            return _class;
        }

        public void Delete(int id)
        {
            var obj = _db.Class.Find(id);
            _db.Class.Remove(obj);
        }

        public IEnumerable<Class> GetAll()
        {
            var obj = _db.Class;
            return obj;
        }

        public Class GetByID(int id)
        {
            var obj = _db.Class.Find(id);
            return obj;
        }

        public Class Update(Class _class)
        {
            var obj = _db.Class.Find(_class.ClassID);
            if (obj == null)
                throw new AppException("Class is not found");
            if (!string.IsNullOrWhiteSpace(_class.ClassName))
                obj.ClassName = _class.ClassName;
            _db.Class.Update(obj);
            _db.SaveChanges();
            return obj;
        }
    }

}
