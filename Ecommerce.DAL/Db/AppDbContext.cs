using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasMany(r => r.Users).WithOne(u => u.Role).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, RoleName = "admin" },
            new Role { Id = 2, RoleName = "instructor" }, new Role { Id = 3, RoleName = "parent" }, new Role { Id = 4, RoleName = "student" });
            modelBuilder.Entity<Course>()
       .Property(c => c.Created)
       .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Course>()
       .Property(c => c.Updated)
       .HasDefaultValueSql("GETDATE()");
        }
    }
}
