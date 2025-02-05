using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data.Models
{
    public class StudentSystemContext : DbContext
    {
        private const string ConnectionString =
            "Server=.;Database=StudentSystem;Trusted_Connection=True;"
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            :base(options)
        {

        }
        DbSet<Student> Students { get; set; }
        DbSet<Homework> Homeworks { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<StudentCourse> StudentsCourses { get; set; }
    }
}
