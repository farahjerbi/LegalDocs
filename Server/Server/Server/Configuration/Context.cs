using Microsoft.EntityFrameworkCore;
using Server.Domain;

namespace Server.Configuration
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Log_Doc> Log_Docs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Alias> Aliases { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<TypeSettings> TypeSettings { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Template>()
                .HasOne(e => e.Log_Doc)
            .WithMany(e => e.Templates)
                .HasForeignKey(e => e.Log_Doc_FK)
                .OnDelete(DeleteBehavior.Cascade); 


            modelBuilder.Entity<Alias>()
               .HasOne(e => e.Group)
           .WithMany(e => e.Aliases)
               .HasForeignKey(e => e.Group_FK)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
               .HasOne(e => e.Template)
            .WithMany(e => e.Groups)
               .HasForeignKey(e => e.Template_FK)
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Template>()
               .HasOne(e => e.Language)
           .WithMany(e => e.Templates)
               .HasForeignKey(e => e.Language_FK)
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Alias>()
                .OwnsOne(a => a.TypeSetting);


            modelBuilder.Entity<Alias>()
            .HasMany(a => a.Users)
            .WithMany(u => u.Aliases)
            .UsingEntity(c => c.ToTable("UsersAliases"));



        }

    }
    }
