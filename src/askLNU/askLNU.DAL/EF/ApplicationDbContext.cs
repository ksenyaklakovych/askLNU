using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace askLNU.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUserFavoriteQuestion>()
                .HasKey(o => new { o.ApplicationUserId, o.QuestionId });

            builder.Entity<ApplicationUserLabel>()
                .HasKey(o => new { o.ApplicationUserId, o.LabelId });

            builder.Entity<QuestionTag>()
                .HasKey(o => new { o.QuestionId, o.TagId });

            // Seed data
            builder.Entity<Faculty>().HasData(
                new Faculty[]
                {
                    new Faculty { Id = 1, Title = "Applied mathematics and computer science" },
                    new Faculty { Id = 2, Title = "Electronics" },
                    new Faculty { Id = 3, Title = "Philology" },
                    new Faculty { Id = 4, Title = "Mechanics and Mathematics" }
                }
            );

            builder.Entity<Tag>().HasData(
                new Tag[]
                {
                    new Tag { Id = 1, Text = "навчальний процес" },
                    new Tag { Id = 2, Text = "сесія" },
                    new Tag { Id = 3, Text = "стипендія" },
                    new Tag { Id = 4, Text = "студентська рада" },
                    new Tag { Id = 5, Text = "бібліотека" },
                    new Tag { Id = 6, Text = "спортивні секції" },
                    new Tag { Id = 7, Text = "медогляд" }
                }
            );

            base.OnModelCreating(builder);
        }
    }
}
