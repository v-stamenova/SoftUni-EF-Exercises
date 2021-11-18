using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
	public class StudentSystemContext : DbContext
	{
		public StudentSystemContext()
		{

		}

		public StudentSystemContext(DbContextOptions options) : base(options)
		{
				
		}

		public DbSet<Course> Courses { get; set; }

		public DbSet<Homework> HomeworkSubmissions { get; set; }

		public DbSet<Resource> Resources { get; set; }

		public DbSet<Student> Students { get; set; }

		public DbSet<StudentCourse> StudentCourses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Course>(entity =>
			{
				entity.HasKey(e => e.CourseId);

				entity.Property(e => e.Name)
					.IsRequired()
					.IsUnicode()
					.HasMaxLength(80);

				entity.Property(e => e.Description)
					.IsRequired(false)
					.IsUnicode();

				entity.Property(e => e.StartDate)
					.IsRequired();

				entity.Property(e => e.EndDate)
					.IsRequired();

				entity.Property(e => e.Price)
					.IsRequired();
			});

			modelBuilder.Entity<Homework>(entity =>
			{
				entity.HasKey(e => e.HomeworkId);

				entity.Property(e => e.Content)
					.IsRequired()
					.IsUnicode(false);

				entity.Property(e => e.ContentType)
					.IsRequired();

				entity.Property(e => e.SubmissionTime)
					.IsRequired();

				entity.HasOne(e => e.Student)
					.WithMany(h => h.HomeworkSubmissions)
					.HasForeignKey(e => e.StudentId);

				entity.HasOne(e => e.Course)
					.WithMany(c => c.HomeworkSubmissions)
					.HasForeignKey(e => e.CourseId);
			});

			modelBuilder.Entity<Resource>(entity =>
			{
				entity.HasKey(e => e.ResourceId);

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(50)
					.IsUnicode();

				entity.Property(e => e.Url)
					.IsRequired()
					.IsUnicode(false);

				entity.Property(e => e.ResourceType)
					.IsRequired();

				entity.HasOne(e => e.Course)
					.WithMany(r => r.Resources)
					.HasForeignKey(e => e.CourseId);
			});

			modelBuilder.Entity<Student>(entity =>
			{
				entity.HasKey(e => e.StudentId);

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode();

				entity.Property(e => e.PhoneNumber)
					.IsRequired(false)
					.HasMaxLength(10);

				entity.Property(e => e.RegisteredOn)
					.IsRequired();

				entity.Property(e => e.Birthday)
					.IsRequired(false);
			});

			modelBuilder.Entity<StudentCourse>(entity =>
			{
				entity.HasKey(e => new { e.StudentId, e.CourseId });

				entity.HasOne(e => e.Student)
					.WithMany(c => c.CourseEnrollments)
					.HasForeignKey(e => e.StudentId);

				entity.HasOne(e => e.Course)
					.WithMany(c => c.StudentsEnrolled)
					.HasForeignKey(e => e.CourseId);
			});
		}
	}
}
