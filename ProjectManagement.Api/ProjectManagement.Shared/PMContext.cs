using Microsoft.EntityFrameworkCore;
using ProjectManagement.Entities;
using System;

namespace ProjectManagement.Shared
{
    public class PMContext : DbContext
    {
        public PMContext(DbContextOptions<PMContext> context): base(context)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { ID = 1, FirstName = "Nitin", LastName = "Kumar", Email = "great@alpha.beta", Password = "alpha" });
            modelBuilder.Entity<User>().HasData(new User { ID = 2, FirstName = "Ram", LastName = "Das", Email = "nitin@alpha.beta", Password = "alpha" });
            modelBuilder.Entity<User>().HasData(new User { ID = 3, FirstName = "Guest", LastName = "", Email = "guest@welcome.in", Password = "alpha" });
            modelBuilder.Entity<Project>().HasData(new Project { ID = 1, Name = "P1", Detail = "Project-1", CreatedOn = DateTime.UtcNow });
            modelBuilder.Entity<Project>().HasData(new Project { ID = 2, Name = "P2", Detail = "Project-2", CreatedOn = DateTime.UtcNow });
            modelBuilder.Entity<Project>().HasData(new Project { ID = 3, Name = "P3", Detail = "Project-3", CreatedOn = DateTime.UtcNow });
            modelBuilder.Entity<Task>().HasData(new Task { ID = 1, ProjectID = 1, Detail = "Task-1", AssignedToUserID = 1, Status = Entities.Enums.TaskStatus.New, CreatedOn = DateTime.UtcNow });
            modelBuilder.Entity<Task>().HasData(new Task { ID = 2, ProjectID = 2, Detail = "Task-2", AssignedToUserID = 2, Status = Entities.Enums.TaskStatus.InProgress, CreatedOn = DateTime.UtcNow });
            modelBuilder.Entity<Task>().HasData(new Task { ID = 3, ProjectID = 2, Detail = "Task-3", AssignedToUserID = 1, Status = Entities.Enums.TaskStatus.Completed, CreatedOn = DateTime.UtcNow });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
