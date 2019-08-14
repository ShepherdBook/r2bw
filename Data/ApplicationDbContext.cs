using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace r2bw.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public DbSet<User> Users { get; set; }

        public DbSet<ParticipantStatus> ParticipantStatus { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<Attendance> Attendance { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseType> PurchaseTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}