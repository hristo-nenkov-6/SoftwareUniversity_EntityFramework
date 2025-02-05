using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcademicRecordsApp.Data.Models;

public partial class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? FullName { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public  ICollection<Course> Courses { get; set; } = new List<Course>();
}
