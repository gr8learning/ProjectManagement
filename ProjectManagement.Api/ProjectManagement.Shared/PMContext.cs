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
        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
