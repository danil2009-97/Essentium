using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.EF
{
    public class MyContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public MyContext(DbContextOptions<MyContext> opt): base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Timesheet>()
                .HasKey(t => new { t.UserID, t.Year, t.Month, t.Day });


            base.OnModelCreating(builder);
        }
    }
}
