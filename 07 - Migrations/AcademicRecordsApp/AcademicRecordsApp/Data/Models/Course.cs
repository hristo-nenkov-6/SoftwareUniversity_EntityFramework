using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicRecordsApp.Data.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
