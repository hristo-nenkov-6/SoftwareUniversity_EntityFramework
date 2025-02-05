using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required, MaxLength(80), Unicode]
        public string Name { get; set; }

        [Unicode]
        public string Description  { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Decimal Price { get; set; }

        public ICollection<Student> Students { get; } = [];
        public ICollection<Resource> Resources { get; } = [];
        public ICollection<Homework> Homeworks { get; } = [];
    }
}
