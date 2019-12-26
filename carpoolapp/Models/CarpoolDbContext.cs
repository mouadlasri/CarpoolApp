using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace carpoolapp.Models
{
    public partial class CarpoolDbContext : DbContext
    {
        public CarpoolDbContext()
        {
        }

        public CarpoolDbContext(DbContextOptions<CarpoolDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserAnalytics> UserAnalytics { get; set; }
        public virtual DbSet<BookingsDetailsTrips> BookingsDetailsTrips { get; set; }
        public virtual DbSet<CommentsTrips> CommentsTrips { get; set; }
        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Credentials> Credentials { get; set; }
        public virtual DbSet<Trip> Trip { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:db-class-mouadlasri.database.windows.net,1433;Initial Catalog=db-class-mouadlasri;Persist Security Info=False;User ID=mouadadmindb;Password=db-class-29;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("pk_app_user");

                entity.HasIndex(e => e.UserId)
                    .HasName("UQ__AppUser__1788CC4D7C3786BA")
                    .IsUnique();

                entity.HasIndex(e => new { e.FirstName, e.LastName })
                    .HasName("ixAppUser");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AppUser)
                    .HasForeignKey<AppUser>(d => d.UserId)
                    .HasConstraintName("fk_app_user_credential_id");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.TripId, e.UserId })
                    .HasName("pk_booking");

                entity.HasIndex(e => new { e.TripId, e.UserId })
                    .HasName("ixBooking");

                entity.Property(e => e.BookStatus)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.BookingId).ValueGeneratedOnAdd();

                entity.Property(e => e.BookingJoinDate).HasColumnType("datetime");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.TripId)
                    .HasConstraintName("fk_booking_trip");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_booking_app_user");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentText)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TimeCreated)
                    .HasColumnName("Time_Created")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.TripId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_trip");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_app_user_comment");
            });

            modelBuilder.Entity<Credentials>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("pk_credentials");

                entity.HasIndex(e => e.Username)
                    .HasName("UQ__Credenti__536C85E4B6C30271")
                    .IsUnique();

                entity.Property(e => e.LastLoggedOut).HasColumnType("datetime");

                entity.Property(e => e.LastLoggedin).HasColumnType("datetime");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasIndex(e => e.TripId)
                    .HasName("UQ__Trip__51DC713FEEFC55D6")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("ixTrip");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("Date_Created")
                    .HasColumnType("datetime");

                entity.Property(e => e.Departure)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DepartureDate).HasColumnType("date");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MeetingPlace)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Rating).HasColumnType("decimal(2, 2)");

                entity.Property(e => e.SmokeFree)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Trip)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_user");
            });
        }
    }
}
