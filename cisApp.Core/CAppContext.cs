﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace cisApp.Core
{
    public partial class CAppContext : DbContext
    {
        public CAppContext()
        {
        }

        public CAppContext(DbContextOptions<CAppContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();
                var connectionString = configuration.GetConnectionString("MSSqlConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public virtual DbSet<TmBank> TmBank { get; set; }
        public virtual DbSet<TmBankAccountType> TmBankAccountType { get; set; }
        public virtual DbSet<TmDistrict> TmDistrict { get; set; }
        public virtual DbSet<TmProvince> TmProvince { get; set; }
        public virtual DbSet<TmSubdistrict> TmSubdistrict { get; set; }
        public virtual DbSet<UserDesigner> UserDesigner { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersPassword> UsersPassword { get; set; }
        public virtual DbSet<TmUserType> TmUserType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TmBank>(entity =>
            {
                entity.ToTable("Tm_Bank");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .HasComment("รหัสธนาคาร");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("ชื่อธนาคาร");

                entity.Property(e => e.NameAbbr)
                    .HasMaxLength(15)
                    .HasComment("ชื่อย่อ");
            });

            modelBuilder.Entity<TmBankAccountType>(entity =>
            {
                entity.ToTable("Tm_BankAccountType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<TmDistrict>(entity =>
            {
                entity.ToTable("Tm_District");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NameEn).HasMaxLength(500);

                entity.Property(e => e.NameTh).HasMaxLength(500);
            });

            modelBuilder.Entity<TmProvince>(entity =>
            {
                entity.ToTable("Tm_Province");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NameEn).HasMaxLength(500);

                entity.Property(e => e.NameTh).HasMaxLength(500);
            });

            modelBuilder.Entity<TmSubdistrict>(entity =>
            {
                entity.ToTable("Tm_Subdistrict");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NameEn).HasMaxLength(500);

                entity.Property(e => e.NameTh).HasMaxLength(500);
            });

            modelBuilder.Entity<UserDesigner>(entity =>
            {
                entity.ToTable("User_Designer");

                entity.Property(e => e.UserDesignerId)
                    .HasColumnName("UserDesignerID")
                    .HasComment("รหัสผู้ออกแบบ")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(10)
                    .HasComment("เลขที่บัญชี");

                entity.Property(e => e.AccountType).HasComment("ประเภทบัญชี: 1=ออมทรัพย์, 2=ประจำ, 3=กระแสรายวัน");

                entity.Property(e => e.BankId)
                    .HasColumnName("BankID")
                    .HasComment("รหัสธนาคาร");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DistrictID")
                    .HasComment("code อำเภอ");

                entity.Property(e => e.PersonalId)
                    .HasColumnName("PersonalID")
                    .HasMaxLength(13)
                    .HasComment("เลขประจำตัวประชาชน");

                entity.Property(e => e.PostCode)
                    .HasMaxLength(5)
                    .HasComment("รหัสไปรษณีย์");

                entity.Property(e => e.ProvinceId)
                    .HasColumnName("ProvinceID")
                    .HasComment("code จังหวัด");

                entity.Property(e => e.SubDistrictId)
                    .HasColumnName("SubDistrictID")
                    .HasComment("code ตำบล");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("รหัสผู้ใช้งาน");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__1788CCAC115CA722");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Tel).HasMaxLength(10);

                entity.Property(e => e.UserType)
                    .HasColumnName("User_Type")
                    .HasComment(@"1 = ผู้ใข้งาน,
2 = นักออกแบบ
,3 = เจ้าหน้าที่");
            });

            modelBuilder.Entity<UsersPassword>(entity =>
            {
                entity.HasKey(e => e.PasswordId)
                    .HasName("PK__Users_Pa__EA7BF0E800DD5553");

                entity.ToTable("Users_Password");

                entity.Property(e => e.PasswordId)
                    .HasColumnName("PasswordID")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<TmUserType>(entity =>
            {
                entity.ToTable("Tm_UserType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
