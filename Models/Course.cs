using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITI_Students_Demo.Models
{
    public class Course
    {
        public int CourseId { set; get; }

        [Required]
        public string CourseName { set; get; }

        public virtual List<StudentCourse> StudentCourses { get; set; }

        public virtual List<DepartmentCourse> DepartmentCourses { get; set; }
    }
}