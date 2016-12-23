namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NamRiderContext : DbContext
    {
        public NamRiderContext()
            : base("name=NamRiderContext")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CriticismDrivingInfo> CriticismDrivingInfoes { get; set; }
        public virtual DbSet<CriticismParkingInfo> CriticismParkingInfoes { get; set; }
        public virtual DbSet<DrivingInfo> DrivingInfoes { get; set; }
        public virtual DbSet<Evaluation> Evaluations { get; set; }
        public virtual DbSet<GeographicPoint> GeographicPoints { get; set; }
        public virtual DbSet<ParkingInfo> ParkingInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.CriticismDrivingInfoes)
                .WithRequired(e => e.UserCritismDriving)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.CriticismParkingInfoes)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.DrivingInfoes)
                .WithRequired(e => e.UserPublication)
                .HasForeignKey(e => e.IdUserPublication)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Evaluations)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.ParkingInfoes)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.IdUserPublication)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DrivingInfo>()
                .Property(e => e.AdditionalInfo)
                .IsUnicode(false);

            modelBuilder.Entity<DrivingInfo>()
                .Property(e => e.Latitude)
                .HasPrecision(8, 6);

            modelBuilder.Entity<DrivingInfo>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<DrivingInfo>()
                .Property(e => e.StreetName)
                .IsUnicode(false);

            modelBuilder.Entity<DrivingInfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<DrivingInfo>()
                .HasMany(e => e.CriticismDrivingInfoes)
                .WithRequired(e => e.DrivingInfoCritism)
                .HasForeignKey(e => e.IdDriving)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DrivingInfo>()
                .HasMany(e => e.Evaluations)
                .WithRequired(e => e.DrivingInfo)
                .HasForeignKey(e => e.IdDriving)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DrivingInfo>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.DrivingInfoesAlerts)
                .Map(m => m.ToTable("AlertDrivingInfo").MapLeftKey("IdDriving").MapRightKey("UserId"));

            modelBuilder.Entity<GeographicPoint>()
                .Property(e => e.Latitude)
                .HasPrecision(8, 6);

            modelBuilder.Entity<GeographicPoint>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<ParkingInfo>()
                .Property(e => e.Rayon)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ParkingInfo>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ParkingInfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ParkingInfo>()
                .HasMany(e => e.CriticismParkingInfoes)
                .WithRequired(e => e.ParkingInfo)
                .HasForeignKey(e => e.IdParking)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParkingInfo>()
                .HasMany(e => e.GeographicPoints)
                .WithRequired(e => e.ParkingInfo)
                .HasForeignKey(e => e.IdParking)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParkingInfo>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.ParkingInfoesAlerts)
                .Map(m => m.ToTable("AlertParkingInfo").MapLeftKey("IdParking").MapRightKey("UserId"));
        }
    }
}
