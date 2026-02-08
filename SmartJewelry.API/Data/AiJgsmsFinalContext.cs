using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartJewelry.API.Entities;

namespace SmartJewelry.API.Data;

public partial class AiJgsmsFinalContext : DbContext
{
    public AiJgsmsFinalContext()
    {
    }

    public AiJgsmsFinalContext(DbContextOptions<AiJgsmsFinalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<ConsultationAudio> ConsultationAudios { get; set; }

    public virtual DbSet<ConsultationTicket> ConsultationTickets { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<ContentCreator> ContentCreators { get; set; }

    public virtual DbSet<CustomOrderDetail> CustomOrderDetails { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }

    public virtual DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

    public virtual DbSet<Gemstone> Gemstones { get; set; }

    public virtual DbSet<GoldRate> GoldRates { get; set; }

    public virtual DbSet<GoodsReceiptNote> GoodsReceiptNotes { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryManager> InventoryManagers { get; set; }

    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    public virtual DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PublishRequest> PublishRequests { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<SalesConfig> SalesConfigs { get; set; }

    public virtual DbSet<SalesStaff> SalesStaffs { get; set; }

    public virtual DbSet<StoreManager> StoreManagers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SystemConfig> SystemConfigs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-K31BK4E\\HOANGCHATBOY;Initial Catalog=ai_jgsms_final; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Activity__9E2397E034B35B20");

            entity.ToTable("ActivityLog");

            entity.HasIndex(e => e.ActionType, "idx_activitylog_action");

            entity.HasIndex(e => e.EntityName, "idx_activitylog_entity");

            entity.HasIndex(e => e.EntityId, "idx_activitylog_entityid");

            entity.HasIndex(e => e.ActionTimestamp, "idx_activitylog_timestamp");

            entity.HasIndex(e => e.UserId, "idx_activitylog_user");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("action_timestamp");
            entity.Property(e => e.ActionType)
                .HasMaxLength(20)
                .HasColumnName("action_type");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityName)
                .HasMaxLength(100)
                .HasColumnName("entity_name");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.NewValuesJson).HasColumnName("new_values_json");
            entity.Property(e => e.OldValuesJson).HasColumnName("old_values_json");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ActivityLog_User");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__CAA247C8B16DDBDE");

            entity.ToTable("Address");

            entity.HasIndex(e => e.CustomerId, "IX_Address_CustomerId");

            entity.HasIndex(e => new { e.CustomerId, e.IsDefault }, "IX_Address_IsDefault");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AddressLine)
                .HasMaxLength(200)
                .HasColumnName("address_line");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CityCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("city_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.DistrictCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("district_code");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RecipientName)
                .HasMaxLength(100)
                .HasColumnName("recipient_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Ward)
                .HasMaxLength(100)
                .HasColumnName("ward");
            entity.Property(e => e.WardCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ward_code");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Address_Customer");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__43AA414143435989");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.UserId, "UQ__Admin__B9BE370E63942E52").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.PermissionLevel)
                .HasMaxLength(20)
                .HasDefaultValue("admin")
                .HasColumnName("permission_level");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admin_User");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__2EF52A27C8354005");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.CustomerId, "idx_cart_customer");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Items).HasColumnName("items");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Customer");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B423D1F566");

            entity.ToTable("Category");

            entity.HasIndex(e => e.CategoryName, "idx_category_name");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("display_order");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Category_Parent");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__Collecti__53D3A5CA80F3D9A7");

            entity.ToTable("Collection");

            entity.HasIndex(e => e.CollectionType, "idx_collection_type");

            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.BannerImageUrl)
                .HasMaxLength(500)
                .HasColumnName("banner_image_url");
            entity.Property(e => e.CollectionName)
                .HasMaxLength(200)
                .HasColumnName("collection_name");
            entity.Property(e => e.CollectionType)
                .HasMaxLength(20)
                .HasColumnName("collection_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("display_order");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Products).HasColumnName("products");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Collections)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Collection_Creator");
        });

        modelBuilder.Entity<ConsultationAudio>(entity =>
        {
            entity.HasKey(e => e.AudioId).HasName("PK__Consulta__D71A93E78372A902");

            entity.ToTable("ConsultationAudio");

            entity.HasIndex(e => e.TicketId, "idx_audio_ticket");

            entity.HasIndex(e => e.TranscriptionStatus, "idx_audio_transcription");

            entity.Property(e => e.AudioId).HasColumnName("audio_id");
            entity.Property(e => e.AudioDurationSeconds).HasColumnName("audio_duration_seconds");
            entity.Property(e => e.AudioUrl)
                .HasMaxLength(500)
                .HasColumnName("audio_url");
            entity.Property(e => e.ExtractedAt).HasColumnName("extracted_at");
            entity.Property(e => e.Extraction).HasColumnName("extraction");
            entity.Property(e => e.ExtractionStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("extraction_status");
            entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
            entity.Property(e => e.ReviewedBySales)
                .HasDefaultValue(false)
                .HasColumnName("reviewed_by_sales");
            entity.Property(e => e.SalesConfirmedData).HasColumnName("sales_confirmed_data");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.TranscribedAt).HasColumnName("transcribed_at");
            entity.Property(e => e.Transcript).HasColumnName("transcript");
            entity.Property(e => e.TranscriptionStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("transcription_status");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("uploaded_at");
            entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");

            entity.HasOne(d => d.Ticket).WithMany(p => p.ConsultationAudios)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audio_Ticket");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.ConsultationAudios)
                .HasForeignKey(d => d.UploadedBy)
                .HasConstraintName("FK_Audio_Sales");
        });

        modelBuilder.Entity<ConsultationTicket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Consulta__D596F96B1B0A9B3C");

            entity.ToTable("ConsultationTicket");

            entity.HasIndex(e => e.TicketNumber, "UQ__Consulta__413613D2F0D70430").IsUnique();

            entity.HasIndex(e => e.Category, "idx_ticket_category");

            entity.HasIndex(e => e.CustomerId, "idx_ticket_customer");

            entity.HasIndex(e => e.TicketNumber, "idx_ticket_number");

            entity.HasIndex(e => e.SalesStaffId, "idx_ticket_sales");

            entity.HasIndex(e => e.Status, "idx_ticket_status");

            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.AssignedAt).HasColumnName("assigned_at");
            entity.Property(e => e.AssignmentHistory).HasColumnName("assignment_history");
            entity.Property(e => e.AutoAssigned)
                .HasDefaultValue(false)
                .HasColumnName("auto_assigned");
            entity.Property(e => e.Category)
                .HasMaxLength(30)
                .HasDefaultValue("general")
                .HasColumnName("category");
            entity.Property(e => e.CreationTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("creation_time");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("last_updated");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("medium")
                .HasColumnName("priority");
            entity.Property(e => e.ResolvedAt).HasColumnName("resolved_at");
            entity.Property(e => e.SalesStaffId).HasColumnName("sales_staff_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("open")
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(200)
                .HasColumnName("subject");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(50)
                .HasColumnName("ticket_number");

            entity.HasOne(d => d.Customer).WithMany(p => p.ConsultationTickets)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Customer");

            entity.HasOne(d => d.SalesStaff).WithMany(p => p.ConsultationTickets)
                .HasForeignKey(d => d.SalesStaffId)
                .HasConstraintName("FK_Ticket_Sales");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__Content__655FE510E036FEE2");

            entity.ToTable("Content");

            entity.HasIndex(e => e.UrlSlug, "UQ__Content__586CCF1FFC1E307C").IsUnique();

            entity.HasIndex(e => e.UrlSlug, "idx_content_slug");

            entity.HasIndex(e => e.PublicationStatus, "idx_content_status");

            entity.HasIndex(e => e.ContentType, "idx_content_type");

            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.ContentBody).HasColumnName("content_body");
            entity.Property(e => e.ContentTitle)
                .HasMaxLength(300)
                .HasColumnName("content_title");
            entity.Property(e => e.ContentType)
                .HasMaxLength(20)
                .HasColumnName("content_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Excerpt).HasColumnName("excerpt");
            entity.Property(e => e.FeaturedImageUrl)
                .HasMaxLength(500)
                .HasColumnName("featured_image_url");
            entity.Property(e => e.PublicationStatus)
                .HasMaxLength(20)
                .HasDefaultValue("draft")
                .HasColumnName("publication_status");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.SeoMetadata).HasColumnName("seo_metadata");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UrlSlug)
                .HasMaxLength(300)
                .HasColumnName("url_slug");
            entity.Property(e => e.ViewCount)
                .HasDefaultValue(0)
                .HasColumnName("view_count");

            entity.HasOne(d => d.Author).WithMany(p => p.Contents)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Content_Author");
        });

        modelBuilder.Entity<ContentCreator>(entity =>
        {
            entity.HasKey(e => e.ContentCreatorId).HasName("PK__ContentC__32C10A34767B7265");

            entity.ToTable("ContentCreator");

            entity.HasIndex(e => e.UserId, "UQ__ContentC__B9BE370E45D164BC").IsUnique();

            entity.Property(e => e.ContentCreatorId).HasColumnName("content_creator_id");
            entity.Property(e => e.ContentCount)
                .HasDefaultValue(0)
                .HasColumnName("content_count");
            entity.Property(e => e.SpecialtyArea)
                .HasMaxLength(100)
                .HasColumnName("specialty_area");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.ContentCreator)
                .HasForeignKey<ContentCreator>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContentCreator_User");
        });

        modelBuilder.Entity<CustomOrderDetail>(entity =>
        {
            entity.HasKey(e => e.CustomDetailId).HasName("PK__CustomOr__30F332288D1E0425");

            entity.ToTable("CustomOrderDetail");

            entity.HasIndex(e => e.OrderId, "UQ__CustomOr__4659622863D8C12F").IsUnique();

            entity.HasIndex(e => e.OrderId, "idx_customdetail_order");

            entity.Property(e => e.CustomDetailId).HasColumnName("custom_detail_id");
            entity.Property(e => e.ActualCompletionDate).HasColumnName("actual_completion_date");
            entity.Property(e => e.ConsultationTicketId).HasColumnName("consultation_ticket_id");
            entity.Property(e => e.EstimatedCompletionDate).HasColumnName("estimated_completion_date");
            entity.Property(e => e.Modifications).HasColumnName("modifications");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.SelectedGemstoneId).HasColumnName("selected_gemstone_id");
            entity.Property(e => e.SelectedMountingId).HasColumnName("selected_mounting_id");
            entity.Property(e => e.WorkflowStages).HasColumnName("workflow_stages");

            entity.HasOne(d => d.Order).WithOne(p => p.CustomOrderDetail)
                .HasForeignKey<CustomOrderDetail>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomDetail_Order");

            entity.HasOne(d => d.SelectedGemstone).WithMany(p => p.CustomOrderDetails)
                .HasForeignKey(d => d.SelectedGemstoneId)
                .HasConstraintName("FK_CustomDetail_Gemstone");

            entity.HasOne(d => d.SelectedMounting).WithMany(p => p.CustomOrderDetails)
                .HasForeignKey(d => d.SelectedMountingId)
                .HasConstraintName("FK_CustomDetail_Mounting");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85F4E0FBD2");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserId, "UQ__Customer__B9BE370E77F9C142").IsUnique();

            entity.HasIndex(e => e.LoyaltyPoints, "idx_customer_loyalty");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerTier)
                .HasMaxLength(20)
                .HasDefaultValue("bronze")
                .HasColumnName("customer_tier");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.LoyaltyPoints)
                .HasDefaultValue(0)
                .HasColumnName("loyalty_points");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_User");
        });

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Customer__AEBB701F8E206395");

            entity.ToTable("CustomerProfile");

            entity.HasIndex(e => e.CustomerId, "UQ__Customer__CD65CB849B9A2DE5").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Addresses).HasColumnName("addresses");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Preferences).HasColumnName("preferences");
            entity.Property(e => e.RingSizes).HasColumnName("ring_sizes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Vouchers).HasColumnName("vouchers");

            entity.HasOne(d => d.Customer).WithOne(p => p.CustomerProfile)
                .HasForeignKey<CustomerProfile>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerProfile_Customer");
        });

        modelBuilder.Entity<EmailVerificationToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__EmailVer__CB3C9E17B4F2E60B");

            entity.ToTable("EmailVerificationToken");

            entity.HasIndex(e => e.ExpiresAt, "IX_EmailVerificationToken_ExpiresAt");

            entity.HasIndex(e => e.Token, "IX_EmailVerificationToken_Token");

            entity.HasIndex(e => new { e.UserId, e.IsUsed }, "IX_EmailVerificationToken_UserId_IsUsed");

            entity.HasIndex(e => e.Token, "UQ__EmailVer__CA90DA7A50A6BB1F").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ip_address");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .HasColumnName("token");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.EmailVerificationTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_EmailVerificationToken_User");
        });

        modelBuilder.Entity<Gemstone>(entity =>
        {
            entity.HasKey(e => e.GemstoneId).HasName("PK__Gemstone__08CCD219B2328631");

            entity.ToTable("Gemstone");

            entity.HasIndex(e => e.GemstoneCode, "UQ__Gemstone__46E50EA33B53E1EB").IsUnique();

            entity.HasIndex(e => e.GemstoneCode, "idx_gemstone_code");

            entity.HasIndex(e => e.GemstoneStatus, "idx_gemstone_status");

            entity.HasIndex(e => e.GemstoneType, "idx_gemstone_type");

            entity.Property(e => e.GemstoneId).HasColumnName("gemstone_id");
            entity.Property(e => e.CertFileUrl)
                .HasMaxLength(500)
                .HasColumnName("cert_file_url");
            entity.Property(e => e.CertificateLab)
                .HasMaxLength(100)
                .HasColumnName("certificate_lab");
            entity.Property(e => e.CertificateNumber)
                .HasMaxLength(100)
                .HasColumnName("certificate_number");
            entity.Property(e => e.ClarityGrade)
                .HasMaxLength(20)
                .HasColumnName("clarity_grade");
            entity.Property(e => e.ColorGrade)
                .HasMaxLength(20)
                .HasColumnName("color_grade");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CutGrade)
                .HasMaxLength(20)
                .HasColumnName("cut_grade");
            entity.Property(e => e.Fluorescence)
                .HasMaxLength(50)
                .HasColumnName("fluorescence");
            entity.Property(e => e.GemstoneCode)
                .HasMaxLength(50)
                .HasColumnName("gemstone_code");
            entity.Property(e => e.GemstoneStatus)
                .HasMaxLength(20)
                .HasDefaultValue("available")
                .HasColumnName("gemstone_status");
            entity.Property(e => e.GemstoneType)
                .HasMaxLength(100)
                .HasColumnName("gemstone_type");
            entity.Property(e => e.Image360Url)
                .HasMaxLength(500)
                .HasColumnName("image_360_url");
            entity.Property(e => e.MarkupPercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("markup_percentage");
            entity.Property(e => e.OriginCountry)
                .HasMaxLength(100)
                .HasColumnName("origin_country");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("purchase_price");
            entity.Property(e => e.ReservedUntil).HasColumnName("reserved_until");
            entity.Property(e => e.SellingPrice)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("selling_price");
            entity.Property(e => e.Shape)
                .HasMaxLength(50)
                .HasColumnName("shape");
            entity.Property(e => e.Treatment)
                .HasMaxLength(100)
                .HasColumnName("treatment");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(500)
                .HasColumnName("video_url");
            entity.Property(e => e.WeightCarats)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("weight_carats");
        });

        modelBuilder.Entity<GoldRate>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__GoldRate__75920B428A47A29F");

            entity.ToTable("GoldRate");

            entity.HasIndex(e => e.EffectiveDate, "idx_goldrate_date");

            entity.HasIndex(e => e.GoldType, "idx_goldrate_type");

            entity.Property(e => e.RateId).HasColumnName("rate_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("effective_date");
            entity.Property(e => e.GoldType)
                .HasMaxLength(10)
                .HasColumnName("gold_type");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.RatePerGram)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("rate_per_gram");
            entity.Property(e => e.RateSource)
                .HasMaxLength(100)
                .HasColumnName("rate_source");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GoldRates)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_GoldRate_Admin");
        });

        modelBuilder.Entity<GoodsReceiptNote>(entity =>
        {
            entity.HasKey(e => e.GrnId).HasName("PK__GoodsRec__39D8A22A96C0C122");

            entity.ToTable("GoodsReceiptNote");

            entity.HasIndex(e => e.GrnNumber, "UQ__GoodsRec__4C7026B2C4333518").IsUnique();

            entity.HasIndex(e => e.GrnNumber, "idx_grn_number");

            entity.HasIndex(e => e.ReceiptStatus, "idx_grn_status");

            entity.Property(e => e.GrnId).HasColumnName("grn_id");
            entity.Property(e => e.GrnNumber)
                .HasMaxLength(50)
                .HasColumnName("grn_number");
            entity.Property(e => e.InventoryManagerId).HasColumnName("inventory_manager_id");
            entity.Property(e => e.Lines).HasColumnName("lines");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PostedAt).HasColumnName("posted_at");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.ReceiptDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("receipt_date");
            entity.Property(e => e.ReceiptStatus)
                .HasMaxLength(20)
                .HasDefaultValue("draft")
                .HasColumnName("receipt_status");

            entity.HasOne(d => d.InventoryManager).WithMany(p => p.GoodsReceiptNotes)
                .HasForeignKey(d => d.InventoryManagerId)
                .HasConstraintName("FK_GRN_Manager");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.GoodsReceiptNotes)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GRN_PO");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__B59ACC497C0AB1DD");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.ProductId, "idx_inventory_product");

            entity.HasIndex(e => e.StockedSince, "idx_inventory_stocked");

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.CurrentStock).HasColumnName("current_stock");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("last_updated");
            entity.Property(e => e.MinimumStockThreshold)
                .HasDefaultValue(10)
                .HasColumnName("minimum_stock_threshold");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.StockedSince).HasColumnName("stocked_since");
            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(100)
                .HasColumnName("warehouse_location");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Product");
        });

        modelBuilder.Entity<InventoryManager>(entity =>
        {
            entity.HasKey(e => e.InventoryManagerId).HasName("PK__Inventor__848A5E72A7C187A2");

            entity.ToTable("InventoryManager");

            entity.HasIndex(e => e.UserId, "UQ__Inventor__B9BE370E5D77A623").IsUnique();

            entity.Property(e => e.InventoryManagerId).HasColumnName("inventory_manager_id");
            entity.Property(e => e.CertificationLevel)
                .HasMaxLength(50)
                .HasColumnName("certification_level");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WarehouseLocation)
                .HasMaxLength(200)
                .HasColumnName("warehouse_location");

            entity.HasOne(d => d.User).WithOne(p => p.InventoryManager)
                .HasForeignKey<InventoryManager>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryManager_User");
        });

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Inventor__85C600AF3EA24A56");

            entity.ToTable("InventoryTransaction");

            entity.HasIndex(e => e.TransactionDate, "idx_invtrans_date");

            entity.HasIndex(e => e.ProductId, "idx_invtrans_product");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.QuantityChange).HasColumnName("quantity_change");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.ReferenceType)
                .HasMaxLength(50)
                .HasColumnName("reference_type");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvTransaction_Product");

            entity.HasOne(d => d.Staff).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_InvTransaction_Staff");
        });

        modelBuilder.Entity<LoyaltyTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__LoyaltyT__85C600AF6B69C128");

            entity.ToTable("LoyaltyTransaction");

            entity.HasIndex(e => e.CustomerId, "idx_loyalty_customer");

            entity.HasIndex(e => e.TransactionDate, "idx_loyalty_date");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PointsChange).HasColumnName("points_change");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.Customer).WithMany(p => p.LoyaltyTransactions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoyaltyTransaction_Customer");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__465962297849436F");

            entity.ToTable("Order");

            entity.HasIndex(e => e.OrderNumber, "UQ__Order__730E34DF277D375C").IsUnique();

            entity.HasIndex(e => e.CustomerId, "idx_order_customer");

            entity.HasIndex(e => e.OrderNumber, "idx_order_number");

            entity.HasIndex(e => e.OrderStatus, "idx_order_status");

            entity.HasIndex(e => e.OrderType, "idx_order_type");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.GrandTotal)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("grand_total");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(50)
                .HasColumnName("order_number");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(30)
                .HasDefaultValue("pending")
                .HasColumnName("order_status");
            entity.Property(e => e.OrderType)
                .HasMaxLength(20)
                .HasColumnName("order_type");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(50)
                .HasColumnName("promotion_code");
            entity.Property(e => e.SalesStaffId).HasColumnName("sales_staff_id");
            entity.Property(e => e.ShippingInfo).HasColumnName("shipping_info");
            entity.Property(e => e.StatusHistory).HasColumnName("status_history");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("tax_amount");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.SalesStaff).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SalesStaffId)
                .HasConstraintName("FK_Order_Sales");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__3764B6BCAA05F3E0");

            entity.ToTable("OrderItem");

            entity.HasIndex(e => e.OrderId, "idx_orderitem_order");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("unit_price");
            entity.Property(e => e.VariantSku)
                .HasMaxLength(50)
                .HasColumnName("variant_sku");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Product");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__Password__CB3C9E17D2B28646");

            entity.ToTable("PasswordResetToken");

            entity.HasIndex(e => e.ExpiresAt, "IX_PasswordResetToken_ExpiresAt");

            entity.HasIndex(e => e.Token, "IX_PasswordResetToken_Token");

            entity.HasIndex(e => new { e.UserId, e.IsUsed }, "IX_PasswordResetToken_UserId_IsUsed");

            entity.HasIndex(e => e.Token, "UQ__Password__CA90DA7A23397BAD").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ip_address");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .HasColumnName("token");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PasswordResetToken_User");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EAC961F014");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.PaymentNumber, "UQ__Payment__662A5B73A906D049").IsUnique();

            entity.HasIndex(e => e.PaymentNumber, "idx_payment_number");

            entity.HasIndex(e => e.OrderId, "idx_payment_order");

            entity.HasIndex(e => e.PaymentStatus, "idx_payment_status");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.DepositPercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("deposit_percentage");
            entity.Property(e => e.GatewayResponse).HasColumnName("gateway_response");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentAmount)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("payment_amount");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(20)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentNumber)
                .HasMaxLength(50)
                .HasColumnName("payment_number");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("payment_status");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(20)
                .HasColumnName("payment_type");
            entity.Property(e => e.TransactionReference)
                .HasMaxLength(100)
                .HasColumnName("transaction_reference");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF5BFE73C01");

            entity.ToTable("Product");

            entity.HasIndex(e => e.ProductCode, "UQ__Product__AE1A8CC495109CBB").IsUnique();

            entity.HasIndex(e => e.ProductCode, "idx_product_code");

            entity.HasIndex(e => e.ProductType, "idx_product_type");

            entity.HasIndex(e => e.PublishStatus, "idx_publish_status");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Gemstones).HasColumnName("gemstones");
            entity.Property(e => e.GoldKarat)
                .HasMaxLength(10)
                .HasColumnName("gold_karat");
            entity.Property(e => e.GoldWeightGrams)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("gold_weight_grams");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MaterialType)
                .HasMaxLength(100)
                .HasColumnName("material_type");
            entity.Property(e => e.Media360).HasColumnName("media_360");
            entity.Property(e => e.MountingCompatibility).HasColumnName("mounting_compatibility");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductType)
                .HasMaxLength(30)
                .HasColumnName("product_type");
            entity.Property(e => e.PublishStatus)
                .HasMaxLength(20)
                .HasDefaultValue("draft")
                .HasColumnName("publish_status");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.SeoMetadata).HasColumnName("seo_metadata");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Variants).HasColumnName("variants");
            entity.Property(e => e.WeightGrams)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("weight_grams");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__DC9AC955EF7380E3");

            entity.ToTable("ProductImage");

            entity.HasIndex(e => e.ProductId, "idx_productimage_product");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.AltText)
                .HasMaxLength(255)
                .HasColumnName("alt_text");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("display_order");
            entity.Property(e => e.ImageType)
                .HasMaxLength(20)
                .HasDefaultValue("gallery")
                .HasColumnName("image_type");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("uploaded_at");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImage_Product");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__2CB9556B806C872F");

            entity.ToTable("Promotion");

            entity.HasIndex(e => e.PromotionCode, "UQ__Promotio__88875505C738268A").IsUnique();

            entity.HasIndex(e => e.PromotionCode, "idx_promotion_code");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "idx_promotion_dates");

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.ApplicableProducts).HasColumnName("applicable_products");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.CurrentUsageCount)
                .HasDefaultValue(0)
                .HasColumnName("current_usage_count");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(20)
                .HasColumnName("discount_type");
            entity.Property(e => e.DiscountValue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_value");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.MaxDiscountAmount)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("max_discount_amount");
            entity.Property(e => e.MinOrderAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("min_order_amount");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(50)
                .HasColumnName("promotion_code");
            entity.Property(e => e.PromotionName)
                .HasMaxLength(200)
                .HasColumnName("promotion_name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.TotalUsageLimit).HasColumnName("total_usage_limit");
            entity.Property(e => e.UsageHistory).HasColumnName("usage_history");
            entity.Property(e => e.UsageLimitPerCustomer)
                .HasDefaultValue(1)
                .HasColumnName("usage_limit_per_customer");

            entity.HasOne(d => d.Creator).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_Promotion_Creator");

            entity.HasOne(d => d.Manager).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Promotion_Manager");
        });

        modelBuilder.Entity<PublishRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__PublishR__18D3B90FE01DAAA5");

            entity.ToTable("PublishRequest");

            entity.HasIndex(e => e.RequestNumber, "UQ__PublishR__8EF03D9C25C36A85").IsUnique();

            entity.HasIndex(e => e.RequestNumber, "idx_publishreq_number");

            entity.HasIndex(e => e.RequestStatus, "idx_publishreq_status");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.InventoryManagerId).HasColumnName("inventory_manager_id");
            entity.Property(e => e.Items).HasColumnName("items");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.RequestNumber)
                .HasMaxLength(50)
                .HasColumnName("request_number");
            entity.Property(e => e.RequestStatus)
                .HasMaxLength(30)
                .HasDefaultValue("pending")
                .HasColumnName("request_status");
            entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
            entity.Property(e => e.ReviewerNotes).HasColumnName("reviewer_notes");
            entity.Property(e => e.StoreManagerId).HasColumnName("store_manager_id");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("submitted_at");

            entity.HasOne(d => d.InventoryManager).WithMany(p => p.PublishRequests)
                .HasForeignKey(d => d.InventoryManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PublishReq_InvManager");

            entity.HasOne(d => d.StoreManager).WithMany(p => p.PublishRequests)
                .HasForeignKey(d => d.StoreManagerId)
                .HasConstraintName("FK_PublishReq_StoreManager");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__AFCA88E6DC59FB29");

            entity.ToTable("PurchaseOrder");

            entity.HasIndex(e => e.PoNumber, "UQ__Purchase__113075499A775EC8").IsUnique();

            entity.HasIndex(e => e.PoNumber, "idx_po_number");

            entity.HasIndex(e => e.OrderStatus, "idx_po_status");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.ExpectedDeliveryDate).HasColumnName("expected_delivery_date");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(20)
                .HasDefaultValue("draft")
                .HasColumnName("order_status");
            entity.Property(e => e.PoNumber)
                .HasMaxLength(50)
                .HasColumnName("po_number");
            entity.Property(e => e.StoreManagerId).HasColumnName("store_manager_id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.StoreManager).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.StoreManagerId)
                .HasConstraintName("FK_PO_Manager");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Supplier");
        });

        modelBuilder.Entity<PurchaseOrderLine>(entity =>
        {
            entity.HasKey(e => e.PoLineId).HasName("PK__Purchase__331E09223FE13FB9");

            entity.ToTable("PurchaseOrderLine");

            entity.HasIndex(e => e.PurchaseOrderId, "idx_poline_po");

            entity.Property(e => e.PoLineId).HasColumnName("po_line_id");
            entity.Property(e => e.LineNumber).HasColumnName("line_number");
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("([quantity]*[unit_price])", true)
                .HasColumnType("decimal(26, 2)")
                .HasColumnName("line_total");
            entity.Property(e => e.LineType)
                .HasMaxLength(20)
                .HasColumnName("line_type");
            entity.Property(e => e.NewItemSpec).HasColumnName("new_item_spec");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RequiredChecklist).HasColumnName("required_checklist");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderLines)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_POLine_Product");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderLines)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POLine_PO");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__RefreshT__CB3C9E172B42BFEC");

            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.ExpiresAt, "IX_RefreshToken_ExpiresAt");

            entity.HasIndex(e => e.Token, "IX_RefreshToken_Token");

            entity.HasIndex(e => new { e.UserId, e.IsRevoked }, "IX_RefreshToken_UserId_IsRevoked");

            entity.HasIndex(e => e.Token, "UQ__RefreshT__CA90DA7ACA98AEB8").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DeviceInfo)
                .HasMaxLength(500)
                .HasColumnName("device_info");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ip_address");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.ReplacedByToken)
                .HasMaxLength(500)
                .HasColumnName("replaced_by_token");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.RevokedReason)
                .HasMaxLength(500)
                .HasColumnName("revoked_reason");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasColumnName("token");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefreshToken_User");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__60883D90B1F744BB");

            entity.ToTable("Review");

            entity.HasIndex(e => e.ProductId, "idx_review_product");

            entity.HasIndex(e => e.RatingScore, "idx_review_rating");

            entity.HasIndex(e => e.ReviewStatus, "idx_review_status");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.HelpfulVotes)
                .HasDefaultValue(0)
                .HasColumnName("helpful_votes");
            entity.Property(e => e.IsVerifiedPurchase)
                .HasDefaultValue(false)
                .HasColumnName("is_verified_purchase");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RatingScore).HasColumnName("rating_score");
            entity.Property(e => e.ReviewImages).HasColumnName("review_images");
            entity.Property(e => e.ReviewStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("review_status");
            entity.Property(e => e.ReviewText).HasColumnName("review_text");
            entity.Property(e => e.ReviewTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("review_time");
            entity.Property(e => e.ReviewTitle)
                .HasMaxLength(200)
                .HasColumnName("review_title");
            entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
            entity.Property(e => e.UnhelpfulVotes)
                .HasDefaultValue(0)
                .HasColumnName("unhelpful_votes");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Customer");

            entity.HasOne(d => d.Order).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Review_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Product");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ReviewedBy)
                .HasConstraintName("FK_Review_Reviewer");
        });

        modelBuilder.Entity<SalesConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__SalesCon__4AD1BFF1A2DE6063");

            entity.ToTable("SalesConfig");

            entity.HasIndex(e => e.SalesStaffId, "UQ__SalesCon__D326BA4ED162DA9E").IsUnique();

            entity.HasIndex(e => e.IsOnline, "idx_salesconfig_online");

            entity.HasIndex(e => e.CurrentActiveTickets, "idx_salesconfig_tickets");

            entity.Property(e => e.ConfigId).HasColumnName("config_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrentActiveTickets)
                .HasDefaultValue(0)
                .HasColumnName("current_active_tickets");
            entity.Property(e => e.IsOnline)
                .HasDefaultValue(false)
                .HasColumnName("is_online");
            entity.Property(e => e.LastOnlineAt).HasColumnName("last_online_at");
            entity.Property(e => e.MaxActiveTickets)
                .HasDefaultValue(20)
                .HasColumnName("max_active_tickets");
            entity.Property(e => e.PerformanceKpi).HasColumnName("performance_kpi");
            entity.Property(e => e.SalesStaffId).HasColumnName("sales_staff_id");
            entity.Property(e => e.ShiftSchedule).HasColumnName("shift_schedule");
            entity.Property(e => e.Specialties).HasColumnName("specialties");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.SalesStaff).WithOne(p => p.SalesConfig)
                .HasForeignKey<SalesConfig>(d => d.SalesStaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesConfig_Sales");
        });

        modelBuilder.Entity<SalesStaff>(entity =>
        {
            entity.HasKey(e => e.SalesStaffId).HasName("PK__SalesSta__D326BA4F39322511");

            entity.ToTable("SalesStaff");

            entity.HasIndex(e => e.UserId, "UQ__SalesSta__B9BE370E0D2A7733").IsUnique();

            entity.Property(e => e.SalesStaffId).HasColumnName("sales_staff_id");
            entity.Property(e => e.CommissionRate)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("commission_rate");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("department_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.SalesTarget)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("sales_target");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.SalesStaff)
                .HasForeignKey<SalesStaff>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesStaff_User");
        });

        modelBuilder.Entity<StoreManager>(entity =>
        {
            entity.HasKey(e => e.StoreManagerId).HasName("PK__StoreMan__4DB56291A09D5487");

            entity.ToTable("StoreManager");

            entity.HasIndex(e => e.UserId, "UQ__StoreMan__B9BE370E6F81156E").IsUnique();

            entity.Property(e => e.StoreManagerId).HasColumnName("store_manager_id");
            entity.Property(e => e.ManagedDepartment)
                .HasMaxLength(100)
                .HasColumnName("managed_department");
            entity.Property(e => e.SupervisedStaffCount)
                .HasDefaultValue(0)
                .HasColumnName("supervised_staff_count");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.StoreManager)
                .HasForeignKey<StoreManager>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreManager_User");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__6EE594E8F10372CC");

            entity.ToTable("Supplier");

            entity.HasIndex(e => e.SupplierCode, "UQ__Supplier__A82CE4692B3C2BAB").IsUnique();

            entity.HasIndex(e => e.SupplierName, "idx_supplier_name");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ContactPersonName)
                .HasMaxLength(100)
                .HasColumnName("contact_person_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PaymentTerms)
                .HasMaxLength(100)
                .HasColumnName("payment_terms");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(200)
                .HasColumnName("supplier_name");
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .HasColumnName("tax_code");
        });

        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__SystemCo__4AD1BFF13AF53758");

            entity.ToTable("SystemConfig");

            entity.HasIndex(e => e.ConfigKey, "UQ__SystemCo__BDF6033DE77DDBA9").IsUnique();

            entity.HasIndex(e => e.ConfigKey, "idx_config_key");

            entity.Property(e => e.ConfigId).HasColumnName("config_id");
            entity.Property(e => e.ConfigKey)
                .HasMaxLength(100)
                .HasColumnName("config_key");
            entity.Property(e => e.ConfigType)
                .HasMaxLength(20)
                .HasColumnName("config_type");
            entity.Property(e => e.ConfigValue).HasColumnName("config_value");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SystemConfigs)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_SystemConfig_Admin");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370FA730B442");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E61640AF2ABA3").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__User__F3DBC572C7DCD6F6").IsUnique();

            entity.HasIndex(e => e.Email, "idx_user_email");

            entity.HasIndex(e => e.Role, "idx_user_role");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified).HasColumnName("email_verified");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.SocialLoginId)
                .HasMaxLength(255)
                .HasColumnName("social_login_id");
            entity.Property(e => e.SocialLoginProvider)
                .HasMaxLength(50)
                .HasColumnName("social_login_provider");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
