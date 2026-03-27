using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational.Entities;
using wBialyDBAdapter.Database.ObjectRelational.Entities.Message;
using wBialyDBAdapter.Database.ObjectRelational.Entities.User;

namespace wBialyDBAdapter.Database.ObjectRelational
{
    public class ORDB : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Gastro> Gastros { get; set; }

        public DbSet<Tag_Event> EventTags { get; set; }
        public DbSet<Tag_Gastro> GastroTags { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSecurityProfile> UserSecurityProfiles { get; set; }
        public DbSet<LoginAttemptAudit> LoginAttemptAudits { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PartialPassword> PartialPasswords { get; set; }

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

            modelBuilder.Entity<User>()
                .HasKey(x => x.UserId);

            modelBuilder.Entity<UserSecurityProfile>(entity =>
            {
                entity.HasKey(x => x.UserSecurityProfileId);
                entity.HasIndex(x => x.UserId).IsUnique();

                entity.HasOne(x => x.User)
                      .WithOne(x => x.SecurityProfile)
                      .HasForeignKey<UserSecurityProfile>(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.PartialPasswords)
                    .WithOne(x => x.UserSecurityProfile)
                    .HasForeignKey(x => x.UserSecurityProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PartialPassword>().HasKey(x => x.PartialPasswordId);

            modelBuilder.Entity<LoginAttemptAudit>(entity =>
            {
                entity.HasKey(x => x.LoginAttemptAuditId);
                entity.HasOne(x => x.UserSecurityProfile)
                      .WithMany(x => x.LoginAttemptAudits)
                      .HasForeignKey(x => x.UserSecurityProfileId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => x.AttemptedAtUtc);
                entity.HasIndex(x => x.LoginIdentifier);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.User)
                      .WithMany()
                      .HasForeignKey(m => m.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(m => m.CanModify)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("CanModify"));
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
        }
    }
}