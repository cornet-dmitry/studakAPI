using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using studakAPI.Models;

namespace studakAPI.Context
{
    public partial class studakContext : DbContext
    {
        public studakContext()
        {
        }

        public studakContext(DbContextOptions<studakContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PublicActive> PublicActives { get; set; } = null!;
        public virtual DbSet<PublicEvent> PublicEvents { get; set; } = null!;
        public virtual DbSet<PublicInvolvement> PublicInvolvements { get; set; } = null!;
        public virtual DbSet<PublicOrganization> PublicOrganizations { get; set; } = null!;
        public virtual DbSet<PublicOrganizationLevel> PublicOrganizationLevels { get; set; } = null!;
        public virtual DbSet<PublicParticipationStatus> PublicParticipationStatuses { get; set; } = null!;
        public virtual DbSet<PublicUser> PublicUsers { get; set; } = null!;
        public virtual DbSet<PublicWorkDirection> PublicWorkDirections { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=studak;Username=postgres;password=admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicActive>(entity =>
            {
                entity.ToTable("public.Active");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Direction).HasColumnName("direction");

                entity.Property(e => e.Organization).HasColumnName("organization");

                entity.Property(e => e.Position)
                    .HasColumnType("character varying")
                    .HasColumnName("position");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.Property(e => e.User).HasColumnName("user");

                entity.HasOne(d => d.DirectionNavigation)
                    .WithMany(p => p.PublicActives)
                    .HasForeignKey(d => d.Direction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Active_fk2");

                entity.HasOne(d => d.OrganizationNavigation)
                    .WithMany(p => p.PublicActives)
                    .HasForeignKey(d => d.Organization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Active_fk0");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.PublicActives)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Active_fk1");
            });

            modelBuilder.Entity<PublicEvent>(entity =>
            {
                entity.ToTable("public.Event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.Organization).HasColumnName("organization");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.Property(e => e.Responsible).HasColumnName("responsible");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasOne(d => d.OrganizationNavigation)
                    .WithMany(p => p.PublicEvents)
                    .HasForeignKey(d => d.Organization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Event_fk0");

                entity.HasOne(d => d.ResponsibleNavigation)
                    .WithMany(p => p.PublicEvents)
                    .HasForeignKey(d => d.Responsible)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Event_fk1");
            });

            modelBuilder.Entity<PublicInvolvement>(entity =>
            {
                entity.ToTable("public.Involvement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Event).HasColumnName("event");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.User).HasColumnName("user");

                entity.HasOne(d => d.EventNavigation)
                    .WithMany(p => p.PublicInvolvements)
                    .HasForeignKey(d => d.Event)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Involvement_fk0");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.PublicInvolvements)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Involvement_fk2");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.PublicInvolvements)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Involvement_fk1");
            });

            modelBuilder.Entity<PublicOrganization>(entity =>
            {
                entity.ToTable("public.Organization");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnType("character varying")
                    .HasColumnName("address");

                entity.Property(e => e.FullName)
                    .HasColumnType("character varying")
                    .HasColumnName("full_name");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.ShortName)
                    .HasColumnType("character varying")
                    .HasColumnName("short_name");

                entity.HasOne(d => d.LevelNavigation)
                    .WithMany(p => p.PublicOrganizations)
                    .HasForeignKey(d => d.Level)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Organization_fk0");
            });

            modelBuilder.Entity<PublicOrganizationLevel>(entity =>
            {
                entity.ToTable("public.OrganizationLevel");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LevelName)
                    .HasColumnType("character varying")
                    .HasColumnName("level_name");
            });

            modelBuilder.Entity<PublicParticipationStatus>(entity =>
            {
                entity.ToTable("public.ParticipationStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ParticipationName)
                    .HasColumnType("character varying")
                    .HasColumnName("participation_name");
            });

            modelBuilder.Entity<PublicUser>(entity =>
            {
                entity.ToTable("public.User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateBirth).HasColumnName("date_birth");

                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");

                entity.Property(e => e.Kpi).HasColumnName("KPI");

                entity.Property(e => e.Messenger)
                    .HasColumnType("character varying")
                    .HasColumnName("messenger");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");

                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");

                entity.Property(e => e.Patronymic)
                    .HasColumnType("character varying")
                    .HasColumnName("patronymic");

                entity.Property(e => e.Phone)
                    .HasColumnType("character varying")
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .HasColumnType("character varying")
                    .HasColumnName("role");

                entity.Property(e => e.Surname)
                    .HasColumnType("character varying")
                    .HasColumnName("surname");

                entity.Property(e => e.UserLogin)
                    .HasColumnType("character varying")
                    .HasColumnName("user_login");
            });

            modelBuilder.Entity<PublicWorkDirection>(entity =>
            {
                entity.ToTable("public.WorkDirection");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DirectionName)
                    .HasColumnType("character varying")
                    .HasColumnName("direction_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
