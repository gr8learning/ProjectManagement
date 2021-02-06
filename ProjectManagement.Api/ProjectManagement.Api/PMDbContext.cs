using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Entities;

namespace ProjectManagement.Api
{
    public class PMDbContext : DbContext
    {
        public PMDbContext(DbContextOptions<PMDbContext> context) : base(context)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

    }
}
