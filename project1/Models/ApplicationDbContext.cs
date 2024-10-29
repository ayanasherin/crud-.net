using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project1.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=project1;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Phone, "phone_UNIQUE").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Addressline1)
                .HasMaxLength(45)
                .HasColumnName("addressline1");
            entity.Property(e => e.Addressline2)
                .HasMaxLength(45)
                .HasColumnName("addressline2");
            entity.Property(e => e.BachelorDegree).HasMaxLength(45);
            entity.Property(e => e.BachelorGpa)
                .HasPrecision(3, 2)
                .HasColumnName("BachelorGPA");
            entity.Property(e => e.City)
                .HasMaxLength(45)
                .HasColumnName("city");
            entity.Property(e => e.Countrycode)
                .HasMaxLength(5)
                .HasColumnName("countrycode");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(45)
                .HasColumnName("lastname");
            entity.Property(e => e.LearnedAboutUs).HasColumnType("enum('Friend','Internet','Media','Event')");
            entity.Property(e => e.Md).HasMaxLength(45);
            entity.Property(e => e.MdGpa)
                .HasPrecision(3, 2)
                .HasColumnName("MdGPA");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Prefix)
                .HasMaxLength(10)
                .HasColumnName("prefix");
            entity.Property(e => e.Resume).HasColumnType("mediumblob");
            entity.Property(e => e.Role)
                .HasColumnType("enum('student','professional','other')")
                .HasColumnName("role");
            entity.Property(e => e.State)
                .HasMaxLength(45)
                .HasColumnName("state");
            entity.Property(e => e.Token).HasMaxLength(300);
            entity.Property(e => e.Zipcode)
                .HasMaxLength(45)
                .HasColumnName("zipcode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
