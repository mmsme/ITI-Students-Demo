using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_Students_Demo.Models
{
    public class Student
    {
        public int Id { set; get; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { set; get; }
        [Range(18, 40)]
        public int Age { set; get; }
        [StringLength(100, MinimumLength = 2)]
        public string Address { set; get; }

        public string Img { set; get; }

        [ForeignKey("Department")]
        public int? DeptId { get; set; }


        public virtual Department Department { set; get; }

        public virtual List<StudentCourse> StudentCourses { get; set; }
    }
}