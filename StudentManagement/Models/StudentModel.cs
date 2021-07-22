using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class StudentModel
    {
        [Required]
        [DisplayName("Student Name")]
        public string StudentName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int ClassID { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
