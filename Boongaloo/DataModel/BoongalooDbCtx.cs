namespace DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BoongalooDbCtx : DbContext
    {
        public BoongalooDbCtx()
            : base("name=BoongalooDbCtx")
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Radius> Radiuses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Areas)
                .WithMany(e => e.Groups)
                .Map(m => m.ToTable("AreaGroups").MapLeftKey("GroupId").MapRightKey("AreaId"));

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Groups)
                .Map(m => m.ToTable("GroupUsers").MapLeftKey("GroupId").MapRightKey("UserId"));

            modelBuilder.Entity<Language>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Languages)
                .Map(m => m.ToTable("UserLanguаges").MapLeftKey("LanguageId").MapRightKey("UserId"));

            modelBuilder.Entity<Radius>()
                .HasMany(e => e.Areas)
                .WithRequired(e => e.Radius)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tag>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Tag>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.Tags)
                .Map(m => m.ToTable("GroupTags").MapLeftKey("TagId").MapRightKey("GroupId"));

            modelBuilder.Entity<User>()
                .Property(e => e.IdSrvId)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.About)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);
        }
    }
}
