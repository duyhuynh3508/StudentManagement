using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Entities
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required]
        [DisplayName("Student Name")]
        public string StudentName { get; set; }
        [Required]
        public string Address { get; set; }

        [DisplayName("Class ID")]
        public int ClassID { get; set; }

        [DisplayName("Class")]
        public Class Class { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
