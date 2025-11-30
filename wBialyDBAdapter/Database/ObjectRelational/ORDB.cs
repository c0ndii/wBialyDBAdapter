using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational.Entities;

namespace wBialyBezdomnyEdition.Database.ObjectRelational
{
    public class ORDB : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Gastro> Gastros { get; set; }

        public ORDB(DbContextOptions<ORDB> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventTags)
                .WithMany(t => t.Events);

            modelBuilder.Entity<Gastro>()
                .HasMany(g => g.GastroTags)
                .WithMany(t => t.Gastros);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.PostId);

            modelBuilder.Entity<Gastro>()
                .HasKey(g => g.PostId);

            modelBuilder.Entity<Tag_Event>()
                .HasKey(t => t.TagID);

            modelBuilder.Entity<Tag_Gastro>()
                .HasKey(t => t.TagID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
