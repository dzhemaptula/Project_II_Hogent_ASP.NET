using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project_ITLab.Models.Domain;
using Project_ITLab.Models.Domain.DBClasses;
using Project_ITLab.Models.Enums;

namespace Project_ITLab.Data {
    public class Context : IdentityDbContext {
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdminAuth> Admins { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<SessionCalendar> Calendars { get; set; }

        //public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        //public DbSet<SessionLeader> SessionLeaders { get; set; }

        public Context(DbContextOptions options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Session>();
            //builder.Entity<Session>().HasKey(s => s.SessionId);
            builder.Entity<Session>().Property(s => s.Name).IsRequired(true).HasMaxLength(100);
            builder.Entity<Session>().Property(s => s.Description).IsRequired(false);
            builder.Entity<Session>().Ignore(s => s.Timer);
            builder.Entity<Session>().Property(x => x.Speaker).IsRequired(false);

            //builder.Entity<Session>().HasMany(s => s.Leaders).WithOne();
            //builder.Entity<Session>().HasMany(s => s.SessionUsers).WithOne();

            builder.Entity<SessionLeader>();
            builder.Entity<SessionLeader>().HasKey(s => new { s.SessionId, s.UserId });
            builder.Entity<SessionLeader>().HasOne(s => s.User);

            builder.Entity<RegisteredUser>();
            builder.Entity<RegisteredUser>().HasKey(s => new { s.SessionId, s.UserId });
            builder.Entity<RegisteredUser>().HasOne(s => s.User);

            

            var converter = new ValueConverter<UserStatus, int>(
                x => (int) x, 
                x=> (UserStatus) x);
            var roleConverter = new ValueConverter<UserRole, int>(
                x => (int)x,
                x => (UserRole)x);


            builder.Entity<User>();
            builder.Entity<User>().Property(s => s.FirstName).HasMaxLength(50);
            builder.Entity<User>().Property(s => s.LastName).HasMaxLength(50);
            builder.Entity<User>().Property(s => s.CardNumber).IsRequired(false);
            builder.Entity<User>().Property(s => s.ProfilePictureUrl).IsRequired(false);
            builder.Entity<User>().Property(s => s.Status).HasConversion(converter);
            builder.Entity<User>().Property(s => s.Email).IsRequired();
            builder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            builder.Entity<AdminAuth>();
            builder.Entity<AdminAuth>().Property(s => s.UserRole).HasConversion(roleConverter);

            builder.Entity<Feedback>();

            builder.Entity<Announcement>();

            builder.Entity<SessionCalendar>();
        }
        
    }
}
