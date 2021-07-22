using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class ClassModel
    {
        [Required]
        [DisplayName("Class")]
        public string ClassName { get; set; }
    }
}
