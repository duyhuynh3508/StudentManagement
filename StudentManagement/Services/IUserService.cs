using ProductManagement.Helpers;
using StudentManagement.Data;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public interface IUserService
    {
        User Login(User model);
        IEnumerable<User> GetAll();
        User GetByID(int id);
        User Register(User _user);
        User Update(User _user);
        void Delete(int id);
    }
    public class UserService : IUserService
    {
        public StudentDBContext _db;
        public UserService(StudentDBContext db)
        {
            _db = db;
        }
        public void Delete(int id)
        {
            var obj = _db.User.Find(id);
            _db.Remove(id);
        }

        public IEnumerable<User> GetAll()
        {
            var obj = _db.User;
            return obj;
        }

        public User GetByID(int id)
        {
            var obj = _db.User.Find(id);
            return obj;
        }

        public User Login(User model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return null;
            var obj = new User();
            try
            {
                obj = _db.User.SingleOrDefault(x => x.Email == model.Email);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }            
            if (obj == null)
                return null;
            if (!(obj.Password == GetMD5(model.Password)))
                return null;
            return obj;
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public User Register(User _user)
        {
            if(string.IsNullOrWhiteSpace(_user.Password))
                throw new AppException("Password is required");
            if(_db.User.Any(x=>x.UserName == _user.UserName))
                throw new AppException("Username is taken");
            string pass = GetMD5(_user.Password);
            _user.Password = pass;
            _db.User.Add(_user);
            _db.SaveChanges();
            return _user;
        }

        public User Update(User _user)
        {
            var obj = _db.User.Find(_user.ID);
            if(obj == null)
                throw new AppException("User is not found");
            if (!string.IsNullOrWhiteSpace(_user.Email))
                obj.Email = _user.Email;
            if (!string.IsNullOrWhiteSpace(_user.Password))
                obj.Password = GetMD5(_user.Password);
            _db.User.Update(obj);
            _db.SaveChanges();
            return obj;

        }

        
    }
}
