using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_Students_Demo.Models
{
    public class Instructor
    {
        public int Id { set; get; }
        [Required]
        public string name { set; get; }

        [ForeignKey("Department")]
        public int? DeptId { get; set; }


        public virtual Department Department { set; get; }
    }
}