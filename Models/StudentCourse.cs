using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_Students_Demo.Models
{
    public class StudentCourse
    {
        [Key, Column(Order = 0)]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Key, Column(Order = 1)]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [Range(0, 100)]
        public int Degree { set; get; }
    }
}