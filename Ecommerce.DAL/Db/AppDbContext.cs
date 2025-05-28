using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Models.Entities;
namespace Ecommerce.DAL.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }


        public DbSet<Parent> Parents { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Lesson> Lesson { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Classe> Classes { get; set; }

        public DbSet<Location> Locations { get; set; }



        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<LessonProgress> lessonProgresses { get; set; }

        public DbSet<Level> Levels { get; set; }

        public DbSet<Instructor> Instructors { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasMany(r => r.Users).WithOne(u => u.Role).OnDelete(DeleteBehavior.Restrict);
          
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "admin" },
            new Role { Id = 2, Name = "instructor" }, new Role { Id = 3, Name = "parent" }, new Role { Id = 4, Name = "student" });
            modelBuilder.Entity<Parent>()
            .HasOne(p => p.User)
            .WithOne(u => u.Parent)
            .HasForeignKey<Parent>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Parent>().HasMany(p => p.students).WithOne(s => s.Parent).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
           

            modelBuilder.Entity<Course>()

                .Property(c => c.Created)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Course>()
              .Property(c => c.Updated)
              .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Location>()
             .HasIndex(l => l.Name)
             .IsUnique();

            

            modelBuilder.Entity<Location>().HasMany(l => l.Schedules).WithOne(s => s.Location).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>().HasMany(c => c.Schedules).WithOne(s => s.Course).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Classe>().HasMany(c => c.schedules).WithOne(s => s.Classe).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Schedule>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Schedule_Day", "[Day] >= 0 AND [Day] <= 6"));

            modelBuilder.Entity<Schedule>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Schedule_Week", "[Week] >= 1 AND [Week] <= 52"));

            modelBuilder.Entity<Instructor>()
            .HasIndex(i => i.Cin)
            .IsUnique();

            modelBuilder.Entity<Parent>()
.HasIndex(p => p.Cin)
.IsUnique();

            modelBuilder.Entity<LessonProgress>(entity =>
            {
                entity.HasOne(lp => lp.Course)
                    .WithMany()
                    .HasForeignKey(lp => lp.CourseId)
                    .OnDelete(DeleteBehavior.NoAction); // Change to NoAction
            });
        }
    }
}
