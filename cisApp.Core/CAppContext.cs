using Microsoft.EntityFrameworkCore;
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

        public virtual DbSet<AttachFile> AttachFile { get; set; }
        public virtual DbSet<JobPayment> JobPayment { get; set; }
        public virtual DbSet<JobPaymentImg> JobPaymentImg { get; set; }
        public virtual DbSet<TmBank> TmBank { get; set; }
        public virtual DbSet<TmBankAccountType> TmBankAccountType { get; set; }
        public virtual DbSet<TmDistrict> TmDistrict { get; set; }
        public virtual DbSet<TmProvince> TmProvince { get; set; }
        public virtual DbSet<TmSubdistrict> TmSubdistrict { get; set; }
        public virtual DbSet<UserDesigner> UserDesigner { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersPassword> UsersPassword { get; set; }
        public virtual DbSet<TmUserType> TmUserType { get; set; }
        public virtual DbSet<UserDesignerRequest> UserDesignerRequest { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleMenu> RoleMenu { get; set; }
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<AlbumImage> AlbumImage { get; set; } 
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<JobsStatus> JobsStatus { get; set; }
        public virtual DbSet<JobsType> JobsType { get; set; }
        public virtual DbSet<JobsCandidate> JobsCandidate { get; set; }
        public virtual DbSet<JobsCandidateStatus> JobsCandidateStatus { get; set; }
        public virtual DbSet<JobsExamImage> JobsExamImage { get; set; }
        public virtual DbSet<JobsExamType> JobsExamType { get; set; }
        public virtual DbSet<JobsLogs> JobsLogs { get; set; }
        public virtual DbSet<JobsTracking> JobsTracking { get; set; }
        public virtual DbSet<UserDesignerRequestImage> UserDesignerRequestImage { get; set; }
        public virtual DbSet<UserDesignerRequestImageType> UserDesignerRequestImageType { get; set; }
        public virtual DbSet<UserImg> UserImg { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<TmProceedRatio> TmProceedRatio { get; set; }
        public virtual DbSet<TmVatratio> TmVatratio { get; set; }
        public virtual DbSet<UsersResetPassword> UsersResetPassword { get; set; }
        public virtual DbSet<TmCauseCancel> TmCauseCancel { get; set; }
        public virtual DbSet<ChatGroup> ChatGroup { get; set; }
        public virtual DbSet<ChatGroupUser> ChatGroupUser { get; set; }
        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<JobDesignerReview> JobDesignerReview { get; set; }
        public virtual DbSet<UserFavoriteDesigner> UserFavoriteDesigner { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TmProceedRatio>(entity =>
            {
                entity.ToTable("Tm_ProceedRatio");

                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TmVatratio>(entity =>
            {
                entity.ToTable("Tm_VATRatio");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.AlbumId).HasColumnName("AlbumID");

                entity.Property(e => e.AlbumType).HasComment("แบ่งเป็นเป็น 1=ประกวด,2=ส่งงานงวดที่1,3=ส่งงานงวดที่2,4=ส่งงานงวดที่3");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<AlbumImage>(entity =>
            {
                entity.HasKey(e => e.ImgId)
                    .HasName("PK__Album_im__C5BC81860D9FF88F");

                entity.ToTable("Album_image");

                entity.Property(e => e.ImgId)
                    .HasColumnName("ImgID");

                entity.Property(e => e.AlbumId).HasColumnName("AlbumID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<AttachFile>(entity =>
            {
                entity.Property(e => e.AttachFileId)
                    .HasColumnName("AttachFileID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FileName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Path).IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasColumnName("RefID")
                    .HasComment("รหัสอ้างอิงตัวไฟล์");
            });

            modelBuilder.Entity<JobPayment>(entity =>
            {
                entity.HasKey(e => e.JobPayId)
                    .HasName("PK__Job_Paym__F3714D8B4B32CD06");

                entity.ToTable("Job_Payment");

                entity.Property(e => e.JobPayId)
                    .HasColumnName("JobPayID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.Property(e => e.PayStatus).HasComment("รอชำระเงิน =1 , อยู่ระหว่างตรวจสอบ =2 , สำเร็จ  = 3 , ไม่อนุมัติ/คืนเงิน  = 4 ");
            });

            modelBuilder.Entity<JobPaymentImg>(entity =>
            {
                entity.HasKey(e => e.JobPayimgId)
                    .HasName("PK__Job_Paym__FF2C09F07FD0487C");

                entity.ToTable("Job_Payment_img");

                entity.Property(e => e.JobPayimgId)
                    .HasColumnName("JobPayimgID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.JobPayId).HasColumnName("JobPayID");
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK__Jobs__056690E281404330");

                entity.Property(e => e.JobId)
                    .HasColumnName("JobID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CancelId)
                    .HasColumnName("CancelID")
                    .HasComment("รหัสสาเหตุยกเลิก");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EditSubmitCount)
                    .HasDefaultValueSql("((0))")
                    .HasComment("จำนวนการขอแก้ไข");

                entity.Property(e => e.InvPersonalId)
                    .HasColumnName("InvPersonalID")
                    .HasMaxLength(13);

                entity.Property(e => e.JobAreaSize)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("ขนาดพื้นที่ ");

                entity.Property(e => e.JobCaUserId)
                    .HasColumnName("JobCaUserID")
                    .HasComment("designer");

                entity.Property(e => e.JobDescription)
                    .IsRequired()
                    .HasComment("ขอบเขตงาน");

                entity.Property(e => e.JobFinalPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("ราคารวมค่าบริการและภาษี");

                entity.Property(e => e.JobNo)
                    .HasMaxLength(12)
                    .IsFixedLength()
                    .HasComment("รหัสงาน");

                entity.Property(e => e.JobPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("ราคารวมค่างาน");

                entity.Property(e => e.JobPricePerSqM)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("ราคา/ตรม");

                entity.Property(e => e.JobPriceProceed)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("ราคารวมค่าบริการ");

                entity.Property(e => e.JobProceedRatio).HasComment("%ค่าดำเนินการในใบงาน");

                entity.Property(e => e.JobStatus).HasComment("สถานะปัจจุบัน");

                entity.Property(e => e.JobTypeId)
                    .HasColumnName("JobTypeID")
                    .HasComment("รหัสประเภทใบงาน");

                entity.Property(e => e.JobVatratio)
                    .HasColumnName("JobVATRatio")
                    .HasComment("%VAT ในใบงาน");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("customer");
            });

            modelBuilder.Entity<JobsStatus>(entity =>
            {
                entity.HasKey(e => e.JobStatusId)
                    .HasName("PK__Jobs_Sta__EB485721C5515588");

                entity.ToTable("Jobs_Status");

                entity.Property(e => e.JobStatusId)
                    .HasColumnName("JobStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<JobsType>(entity =>
            {
                entity.HasKey(e => e.JobTypeId)
                    .HasName("PK__Jobs_Typ__E1F462AD32C64837");

                entity.ToTable("Jobs_Type");

                entity.Property(e => e.JobTypeId)
                    .HasColumnName("JobTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });


            modelBuilder.Entity<JobsCandidate>(entity =>
            {
                entity.HasKey(e => e.JobCaId)
                    .HasName("PK__Jobs_Can__AC71D945BF445377");

                entity.ToTable("Jobs_Candidate");

                entity.Property(e => e.JobCaId).HasColumnName("JobCaID");

                entity.Property(e => e.CaStatusId).HasColumnName("CaStatusID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLike).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<JobsCandidateStatus>(entity =>
            {
                entity.HasKey(e => e.CaStatusId)
                    .HasName("PK__Jobs_Can__2404128D01BAC747");

                entity.ToTable("Jobs_Candidate_Status");

                entity.Property(e => e.CaStatusId)
                    .HasColumnName("CaStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<JobsExamImage>(entity =>
            {
                entity.HasKey(e => e.JobsExImgId)
                    .HasName("PK__Jobs_Exa__328ED9F6CF678975");

                entity.ToTable("Jobs_Exam_Image");

                entity.Property(e => e.JobsExImgId)
                    .HasColumnName("JobsExImgID")
                    .ValueGeneratedNever();

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobsExTypeId).HasColumnName("JobsExTypeID");
            });

            modelBuilder.Entity<JobsExamType>(entity =>
            {
                entity.HasKey(e => e.JobExTypeId)
                    .HasName("PK__Jobs_Exa__0E22BCC7AB63F2E7");

                entity.ToTable("Jobs_Exam_Type");

                entity.Property(e => e.JobExTypeId)
                    .HasColumnName("JobExTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<JobsLogs>(entity =>
            {
                entity.HasKey(e => e.JoblogId)
                    .HasName("PK__Jobs_log__6B31812E6A2B4D97");

                entity.ToTable("Jobs_logs");

                entity.Property(e => e.JoblogId).HasColumnName("JoblogID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobId).HasColumnName("JobID");
            });

            modelBuilder.Entity<JobsTracking>(entity =>
            {
                entity.HasKey(e => e.JobTrackingId)
                    .HasName("PK__Jobs_Tra__8D48BF76B04130C2");

                entity.ToTable("Jobs_Tracking");

                entity.Property(e => e.JobTrackingId).HasColumnName("JobTrackingID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.StatusDate).HasColumnType("date");
            });

            modelBuilder.Entity<UserDesignerRequestImage>(entity =>
            {
                entity.HasKey(e => e.UserDesignerRequestImgId)
                    .HasName("PK__UserDesi__351BA0D9066A710B");

                entity.Property(e => e.UserDesignerRequestImgId)
                    .HasColumnName("UserDesignerRequestImgID")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserDesignerRequestId).HasColumnName("UserDesignerRequestID");
            });

            modelBuilder.Entity<UserDesignerRequestImageType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UserImg>(entity =>
            {
                entity.ToTable("User_Img");

                entity.Property(e => e.UserImgId)
                    .HasColumnName("UserImgID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.MenuId)
                    .HasColumnName("MenuID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Action).HasMaxLength(255);

                entity.Property(e => e.Controller).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MenuNameEn).HasMaxLength(100);

                entity.Property(e => e.MenuUrl)
                    .HasColumnName("MenuURL")
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.Property(e => e.RoleMenuId)
                    .HasColumnName("RoleMenuID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });
            modelBuilder.Entity<UserDesignerRequest>(entity =>
            {
                entity.Property(e => e.AccountNumber).HasMaxLength(10);

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.Code).HasMaxLength(15);

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.PersonalId)
                    .HasColumnName("PersonalID")
                    .HasMaxLength(13);

                entity.Property(e => e.PostCode).HasMaxLength(5);

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

                entity.Property(e => e.Status).HasComment("1=รอดำเนินการ, 2=อนุมัติ, 3=ไม่อนุมัติ");

                entity.Property(e => e.SubDistrictId).HasColumnName("SubDistrictID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });
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
                    .HasDefaultValueSql("(newid())")
                    .HasComment("รหัสผู้ออกแบบ");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(10)
                    .HasComment("เลขที่บัญชี");

                entity.Property(e => e.AccountType).HasComment("ประเภทบัญชี: 1=ออมทรัพย์, 2=ประจำ, 3=กระแสรายวัน");

                entity.Property(e => e.AreaSqmmax).HasColumnName("AreaSQMMax");

                entity.Property(e => e.AreaSqmrate)
                    .HasColumnName("AreaSQMRate")
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((250))");

                entity.Property(e => e.AreaSqmremain).HasColumnName("AreaSQMRemain");

                entity.Property(e => e.AreaSqmused).HasColumnName("AreaSQMUsed");

                entity.Property(e => e.BankId)
                    .HasColumnName("BankID")
                    .HasComment("รหัสธนาคาร");

                entity.Property(e => e.DistrictId)
                    .HasColumnName("DistrictID")
                    .HasComment("ID อำเภอ");

                entity.Property(e => e.PersonalId)
                    .HasColumnName("PersonalID")
                    .HasMaxLength(13)
                    .HasComment("เลขประจำตัวประชาชน");

                entity.Property(e => e.PostCode)
                    .HasMaxLength(5)
                    .HasComment("รหัสไปรษณีย์");

                entity.Property(e => e.ProvinceId)
                    .HasColumnName("ProvinceID")
                    .HasComment("ID จังหวัด");

                entity.Property(e => e.SubDistrictId)
                    .HasColumnName("SubDistrictID")
                    .HasComment("ID ตำบล");

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
                    .HasDefaultValueSql("(newid())");

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

            modelBuilder.Entity<UsersResetPassword>(entity =>
            {
                entity.HasKey(e => e.UserResetPasswordId)
                    .HasName("PK__Users_Re__DB8DE62E7B8CD4E5");

                entity.ToTable("Users_ResetPassword");

                entity.Property(e => e.UserResetPasswordId)
                    .HasColumnName("UserResetPasswordID")
                    .HasDefaultValueSql("(newid())");


                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.HasKey(e => e.SettingId)
                    .HasName("PK__Settings__54372B1D096553E5");

                entity.Property(e => e.SettingId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Keyword)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TmCauseCancel>(entity =>
            {
                entity.ToTable("Tm_CauseCancel");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
