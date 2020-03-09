using System;
using System.Collections.Generic;
using System.Text;
using askLNU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace askLNU.Data
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

            base.OnModelCreating(builder);
        }
    }
}
