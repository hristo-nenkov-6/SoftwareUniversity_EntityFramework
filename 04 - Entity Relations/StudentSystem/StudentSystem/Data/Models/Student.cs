using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StduentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name {  get; set; }

        [MaxLength(10), MinLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Course> Courses { get; }
        public ICollection<Homework> Homeworks { get; }
    }
}
