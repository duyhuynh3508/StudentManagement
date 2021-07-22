using Microsoft.EntityFrameworkCore;
using StudentManagement.Entities;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Data
{
    public class StudentDBContext : DbContext
    {
        public static List<Student> StudentList { get; set; }
        public StudentDBContext(DbContextOptions<StudentDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(s => s.StudentID);
            modelBuilder.Entity<Class>().HasKey(s => s.ClassID);
            modelBuilder.Entity<User>().HasKey(s => s.ID);
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<StudentManagement.Models.Register> Register { get; set; }
    }
}
