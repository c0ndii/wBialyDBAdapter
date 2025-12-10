using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyDBAdapter.Database.ObjectRelational
{
    public class ORDB : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Gastro> Gastros { get; set; }

        public DbSet<Tag_Event> EventTags { get; set; }
        public DbSet<Tag_Gastro> GastroTags { get; set; }

        public ORDB(DbContextOptions<ORDB> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.HasMany(e => e.EventTags)
                      .WithMany(t => t.Events)
                      .UsingEntity(j => j.ToTable("EventTagsJoin")); 
            });

            modelBuilder.Entity<Gastro>(entity =>
            {
                entity.HasKey(g => g.PostId);

                entity.HasMany(g => g.GastroTags)
                      .WithMany(t => t.Gastros)
                      .UsingEntity(j => j.ToTable("GastroTagsJoin")); 
            });

            modelBuilder.Entity<Tag_Event>()
                .HasKey(t => t.TagID);

            modelBuilder.Entity<Tag_Gastro>()
                .HasKey(t => t.TagID);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
        }
    }
}