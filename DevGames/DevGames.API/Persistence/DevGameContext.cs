﻿using DevGames.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevGames.API.Persistence
{
    public class DevGameContext : DbContext 
    {
        public DevGameContext(DbContextOptions<DevGameContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                 .HasKey(b => b.Id);

            modelBuilder.Entity<Board>()
                .HasMany(b => b.Posts)
                .WithOne()
                .HasForeignKey(p => p.BoardId);

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne()
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
