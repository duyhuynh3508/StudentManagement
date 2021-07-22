using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Entities
{
    public class Class
    {
        [DisplayName("Class ID")]
        public int ClassID { get; set; }

        [Required]
        [DisplayName("Class")]
        public string ClassName { get; set; }

        public List<Student> Students { get; set; }
    }
}
