using System;
using System.Collections.Generic;
using AcademicRecordsApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicRecordsApp.Data;

public partial class AcademicRecordsDbContext : DbContext
{
    public AcademicRecordsDbContext()
    {
    }

    public AcademicRecordsDbContext(DbContextOptions<AcademicRecordsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Course> Courses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=AcademicRecordsDB; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3214EC07884A7AC6");

            entity.Property(e => e.CourseId).IsRequired(false);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grades__3214EC07B66AAF0B");

            entity.Property(e => e.Value).HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Exam).WithMany(p => p.Grades)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Exams");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC076DBA1A8D");

            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(s => s.Students)
            .UsingEntity<Dictionary<string, object>>(
                "StudentsCourses",
                s => s.HasOne<Course>().WithMany().HasForeignKey("CoursesId"),
                s => s.HasOne<Student>().WithMany().HasForeignKey("StudentsId"),
                s =>
                {
                    s.HasKey("CoursesId", "StudentsId");
                    s.ToTable("StudentsCourses");
                });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
