﻿using Blog.Data.Configuration;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Entity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(ApplicationUser))
                {
                    var method = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.ApplyConfiguration), new[] { typeof(IEntityTypeConfiguration<>) });
                    var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                    genericMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(typeof(EntityConfiguration<>).MakeGenericType(entityType.ClrType)) });
                }
            }
        }
    }
}