using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITI_Students_Demo.Models
{
    public class Department
    {
        public int Id { set; get; }

        [Required]
        public string DeptName { set; get; }

        public virtual List<Student> Students { set; get; }
        public virtual List<Instructor> Instructors { set; get; }
        public virtual List<DepartmentCourse> DepartmentCourses { set; get; }
    }
}