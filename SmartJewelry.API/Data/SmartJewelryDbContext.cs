using Microsoft.EntityFrameworkCore;
using SmartJewelry.API.Entities;

namespace SmartJewelry.API.Data;

public class SmartJewelryDbContext : DbContext
{
    public SmartJewelryDbContext(DbContextOptions<SmartJewelryDbContext> options) : base(options)
    {
    }

    // User Management (7 tables)
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<SalesStaff> SalesStaffs { get; set; }
    public DbSet<InventoryManager> InventoryManagers { get; set; }
    public DbSet<StoreManager> StoreManagers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<ContentCreator> ContentCreators { get; set; }

    // Customer Personalization (3 tables)
    public DbSet<CustomerProfile> CustomerProfiles { get; set; }
    public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
    public DbSet<Address> Addresses { get; set; }

    // Product Catalog (5 tables)
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Gemstone> Gemstones { get; set; }
    public DbSet<Collection> Collections { get; set; }

    // Pricing Engine (2 tables)
    public DbSet<GoldRate> GoldRates { get; set; }
    public DbSet<SystemConfig> SystemConfigs { get; set; }

    // Inventory & Procurement (6 tables)
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    public DbSet<GoodsReceiptNote> GoodsReceiptNotes { get; set; }

    // Publishing Workflow (1 table)
    public DbSet<PublishRequest> PublishRequests { get; set; }

    // Cart & Order (5 tables)
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<CustomOrderDetail> CustomOrderDetails { get; set; }

    // Consultation & AI (2 tables)
    public DbSet<ConsultationTicket> ConsultationTickets { get; set; }
    public DbSet<ConsultationAudio> ConsultationAudios { get; set; }

    // Sales Workload (1 table)
    public DbSet<SalesConfig> SalesConfigs { get; set; }

    // Promotion (1 table)
    public DbSet<Promotion> Promotions { get; set; }

    // Content Management (2 tables)
    public DbSet<Content> Contents { get; set; }
    public DbSet<Review> Reviews { get; set; }

    // Audit (1 table)
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    // Authentication Tokens (3 tables)
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========================================
        // USER MANAGEMENT (7 tables)
        // ========================================
        
        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(50).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            entity.Property(e => e.SocialLoginProvider).HasColumnName("social_login_provider").HasMaxLength(50);
            entity.Property(e => e.SocialLoginId).HasColumnName("social_login_id").HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.EmailVerified).HasColumnName("email_verified");
            
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Role);
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");
            entity.HasKey(e => e.CustomerId);
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.LoyaltyPoints).HasColumnName("loyalty_points");
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(10);
            entity.Property(e => e.CustomerTier).HasColumnName("customer_tier").HasMaxLength(20);

            entity.HasOne(e => e.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SalesStaff
        modelBuilder.Entity<SalesStaff>(entity =>
        {
            entity.ToTable("SalesStaff");
            entity.HasKey(e => e.SalesStaffId);
            entity.Property(e => e.SalesStaffId).HasColumnName("sales_staff_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DepartmentName).HasColumnName("department_name").HasMaxLength(100);
            entity.Property(e => e.SalesTarget).HasColumnName("sales_target").HasPrecision(15, 2);
            entity.Property(e => e.CommissionRate).HasColumnName("commission_rate").HasPrecision(5, 2);
            entity.Property(e => e.HireDate).HasColumnName("hire_date");

            entity.HasOne(e => e.User)
                .WithOne(u => u.SalesStaff)
                .HasForeignKey<SalesStaff>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // InventoryManager
        modelBuilder.Entity<InventoryManager>(entity =>
        {
            entity.ToTable("InventoryManager");
            entity.HasKey(e => e.InventoryManagerId);
            entity.Property(e => e.InventoryManagerId).HasColumnName("inventory_manager_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WarehouseLocation).HasColumnName("warehouse_location").HasMaxLength(200);
            entity.Property(e => e.CertificationLevel).HasColumnName("certification_level").HasMaxLength(50);

            entity.HasOne(e => e.User)
                .WithOne(u => u.InventoryManager)
                .HasForeignKey<InventoryManager>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // StoreManager
        modelBuilder.Entity<StoreManager>(entity =>
        {
            entity.ToTable("StoreManager");
            entity.HasKey(e => e.StoreManagerId);
            entity.Property(e => e.StoreManagerId).HasColumnName("store_manager_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ManagedDepartment).HasColumnName("managed_department").HasMaxLength(100);
            entity.Property(e => e.SupervisedStaffCount).HasColumnName("supervised_staff_count");

            entity.HasOne(e => e.User)
                .WithOne(u => u.StoreManager)
                .HasForeignKey<StoreManager>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Admin
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");
            entity.HasKey(e => e.AdminId);
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PermissionLevel).HasColumnName("permission_level").HasMaxLength(20);

            entity.HasOne(e => e.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ContentCreator
        modelBuilder.Entity<ContentCreator>(entity =>
        {
            entity.ToTable("ContentCreator");
            entity.HasKey(e => e.ContentCreatorId);
            entity.Property(e => e.ContentCreatorId).HasColumnName("content_creator_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SpecialtyArea).HasColumnName("specialty_area").HasMaxLength(100);
            entity.Property(e => e.ContentCount).HasColumnName("content_count");

            entity.HasOne(e => e.User)
                .WithOne(u => u.ContentCreator)
                .HasForeignKey<ContentCreator>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ========================================
        // CUSTOMER PERSONALIZATION (2 tables)
        // ========================================

        // CustomerProfile
        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.ToTable("CustomerProfile");
            entity.HasKey(e => e.ProfileId);
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.RingSizes).HasColumnName("ring_sizes");
            entity.Property(e => e.Addresses).HasColumnName("addresses");
            entity.Property(e => e.Preferences).HasColumnName("preferences");
            entity.Property(e => e.Vouchers).HasColumnName("vouchers");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(e => e.Customer)
                .WithOne(c => c.CustomerProfile)
                .HasForeignKey<CustomerProfile>(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // LoyaltyTransaction
        modelBuilder.Entity<LoyaltyTransaction>(entity =>
        {
            entity.ToTable("LoyaltyTransaction");
            entity.HasKey(e => e.TransactionId);
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type").HasMaxLength(20);
            entity.Property(e => e.PointsChange).HasColumnName("points_change");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.LoyaltyTransactions)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");
            entity.HasKey(e => e.AddressId);
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.RecipientName).HasColumnName("recipient_name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
            entity.Property(e => e.AddressLine).HasColumnName("address_line").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Ward).HasColumnName("ward").HasMaxLength(100);
            entity.Property(e => e.WardCode).HasColumnName("ward_code").HasMaxLength(20);
            entity.Property(e => e.District).HasColumnName("district").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DistrictCode).HasColumnName("district_code").HasMaxLength(20);
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100).IsRequired();
            entity.Property(e => e.CityCode).HasColumnName("city_code").HasMaxLength(20);
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ========================================
        // PRODUCT CATALOG (5 tables)
        // ========================================

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name").HasMaxLength(100);
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.HasOne(e => e.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductCode).HasColumnName("product_code").HasMaxLength(50);
            entity.Property(e => e.ProductName).HasColumnName("product_name").HasMaxLength(200);
            entity.Property(e => e.ProductType).HasColumnName("product_type").HasMaxLength(30);
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.BasePrice).HasColumnName("base_price").HasPrecision(15, 2);
            entity.Property(e => e.MaterialType).HasColumnName("material_type").HasMaxLength(100);
            entity.Property(e => e.GoldWeightGrams).HasColumnName("gold_weight_grams").HasPrecision(10, 3);
            entity.Property(e => e.GoldKarat).HasColumnName("gold_karat").HasMaxLength(10);
            entity.Property(e => e.Variants).HasColumnName("variants");
            entity.Property(e => e.Media360).HasColumnName("media_360");
            entity.Property(e => e.MountingCompatibility).HasColumnName("mounting_compatibility");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.SeoMetadata).HasColumnName("seo_metadata");
            entity.Property(e => e.Gemstones).HasColumnName("gemstones");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.WeightGrams).HasColumnName("weight_grams").HasPrecision(10, 2);
            entity.Property(e => e.PublishStatus).HasColumnName("publish_status").HasMaxLength(20);
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.HasIndex(e => e.ProductCode).IsUnique();
            entity.HasIndex(e => e.ProductType);
            entity.HasIndex(e => e.PublishStatus);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ProductImage
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImage");
            entity.HasKey(e => e.ImageId);
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").HasMaxLength(500);
            entity.Property(e => e.ImageType).HasColumnName("image_type").HasMaxLength(20);
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);
            entity.Property(e => e.UploadedAt).HasColumnName("uploaded_at");

            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Gemstone
        modelBuilder.Entity<Gemstone>(entity =>
        {
            entity.ToTable("Gemstone");
            entity.HasKey(e => e.GemstoneId);
            entity.Property(e => e.GemstoneId).HasColumnName("gemstone_id");
            entity.Property(e => e.GemstoneCode).HasColumnName("gemstone_code").HasMaxLength(50);
            entity.Property(e => e.GemstoneType).HasColumnName("gemstone_type").HasMaxLength(100);
            entity.Property(e => e.WeightCarats).HasColumnName("weight_carats").HasPrecision(10, 3);
            entity.Property(e => e.Shape).HasColumnName("shape").HasMaxLength(50);
            entity.Property(e => e.ClarityGrade).HasColumnName("clarity_grade").HasMaxLength(20);
            entity.Property(e => e.ColorGrade).HasColumnName("color_grade").HasMaxLength(20);
            entity.Property(e => e.CutGrade).HasColumnName("cut_grade").HasMaxLength(20);
            entity.Property(e => e.Treatment).HasColumnName("treatment").HasMaxLength(100);
            entity.Property(e => e.OriginCountry).HasColumnName("origin_country").HasMaxLength(100);
            entity.Property(e => e.Fluorescence).HasColumnName("fluorescence").HasMaxLength(50);
            entity.Property(e => e.CertificateNumber).HasColumnName("certificate_number").HasMaxLength(100);
            entity.Property(e => e.CertificateLab).HasColumnName("certificate_lab").HasMaxLength(100);
            entity.Property(e => e.CertFileUrl).HasColumnName("cert_file_url").HasMaxLength(500);
            entity.Property(e => e.Image360Url).HasColumnName("image_360_url").HasMaxLength(500);
            entity.Property(e => e.VideoUrl).HasColumnName("video_url").HasMaxLength(500);
            entity.Property(e => e.PurchasePrice).HasColumnName("purchase_price").HasPrecision(15, 2);
            entity.Property(e => e.SellingPrice).HasColumnName("selling_price").HasPrecision(15, 2);
            entity.Property(e => e.MarkupPercentage).HasColumnName("markup_percentage").HasPrecision(5, 2);
            entity.Property(e => e.GemstoneStatus).HasColumnName("gemstone_status").HasMaxLength(20);
            entity.Property(e => e.ReservedUntil).HasColumnName("reserved_until");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasIndex(e => e.GemstoneCode).IsUnique();
            entity.HasIndex(e => e.GemstoneType);
            entity.HasIndex(e => e.GemstoneStatus);
        });

        // Collection
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("Collection");
            entity.HasKey(e => e.CollectionId);
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");
            entity.Property(e => e.CollectionName).HasColumnName("collection_name").HasMaxLength(200);
            entity.Property(e => e.CollectionType).HasColumnName("collection_type").HasMaxLength(20);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.BannerImageUrl).HasColumnName("banner_image_url").HasMaxLength(500);
            entity.Property(e => e.Products).HasColumnName("products");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(e => e.Creator)
                .WithMany(cc => cc.Collections)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ========================================
        // PRICING ENGINE (2 tables)
        // ========================================
        
        modelBuilder.Entity<GoldRate>(entity =>
        {
            entity.ToTable("GoldRate");
            entity.HasKey(e => e.RateId);
        });
        
        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.ToTable("SystemConfig");
            entity.HasKey(e => e.ConfigId);
        });

        // ========================================
        // INVENTORY & PROCUREMENT (6 tables)
        // ========================================
        
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            entity.HasKey(e => e.InventoryId);
        });
        
        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.ToTable("InventoryTransaction");
            entity.HasKey(e => e.TransactionId);
        });
        
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");
            entity.HasKey(e => e.SupplierId);
        });
        
        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("PurchaseOrder");
            entity.HasKey(e => e.PurchaseOrderId);
        });
        
        modelBuilder.Entity<PurchaseOrderLine>(entity =>
        {
            entity.ToTable("PurchaseOrderLine");
            entity.HasKey(e => e.PoLineId);
        });
        
        modelBuilder.Entity<GoodsReceiptNote>(entity =>
        {
            entity.ToTable("GoodsReceiptNote");
            entity.HasKey(e => e.GrnId);
        });

        // ========================================
        // PUBLISHING WORKFLOW (1 table)
        // ========================================
        
        modelBuilder.Entity<PublishRequest>(entity =>
        {
            entity.ToTable("PublishRequest");
            entity.HasKey(e => e.RequestId);
        });

        // ========================================
        // CART & ORDER (5 tables)
        // ========================================
        
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");
            entity.HasKey(e => e.CartId);
        });
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.HasKey(e => e.OrderId);
        });
        
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem");
            entity.HasKey(e => e.OrderItemId);
        });
        
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");
            entity.HasKey(e => e.PaymentId);
        });
        
        modelBuilder.Entity<CustomOrderDetail>(entity =>
        {
            entity.ToTable("CustomOrderDetail");
            entity.HasKey(e => e.CustomDetailId);
        });

        // ========================================
        // CONSULTATION & AI (2 tables)
        // ========================================
        
        modelBuilder.Entity<ConsultationTicket>(entity =>
        {
            entity.ToTable("ConsultationTicket");
            entity.HasKey(e => e.TicketId);
        });
        
        modelBuilder.Entity<ConsultationAudio>(entity =>
        {
            entity.ToTable("ConsultationAudio");
            entity.HasKey(e => e.AudioId);
        });

        // ========================================
        // SALES WORKLOAD (1 table)
        // ========================================
        
        modelBuilder.Entity<SalesConfig>(entity =>
        {
            entity.ToTable("SalesConfig");
            entity.HasKey(e => e.ConfigId);
        });

        // ========================================
        // PROMOTION (1 table)
        // ========================================
        
        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");
            entity.HasKey(e => e.PromotionId);
        });

        // ========================================
        // CONTENT MANAGEMENT (2 tables)
        // ========================================
        
        modelBuilder.Entity<Content>(entity =>
        {
            entity.ToTable("Content");
            entity.HasKey(e => e.ContentId);
        });
        
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");
            entity.HasKey(e => e.ReviewId);
        });

        // ========================================
        // AUDIT (1 table)
        // ========================================
        
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.ToTable("ActivityLog");
            entity.HasKey(e => e.LogId);
            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.EntityName).HasColumnName("entity_name").HasMaxLength(100);
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.ActionType).HasColumnName("action_type").HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ActionTimestamp).HasColumnName("action_timestamp");
            entity.Property(e => e.OldValuesJson).HasColumnName("old_values_json");
            entity.Property(e => e.NewValuesJson).HasColumnName("new_values_json");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithMany(u => u.ActivityLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ========================================
        // AUTHENTICATION TOKENS (3 tables)
        // ========================================
        
        // PasswordResetToken
        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("PasswordResetToken");
            entity.HasKey(e => e.TokenId);
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token").HasMaxLength(100).IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);

            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.UserId, e.IsUsed });

            entity.HasOne(e => e.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // EmailVerificationToken
        modelBuilder.Entity<EmailVerificationToken>(entity =>
        {
            entity.ToTable("EmailVerificationToken");
            entity.HasKey(e => e.TokenId);
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token").HasMaxLength(100).IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);

            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.UserId, e.IsUsed });

            entity.HasOne(e => e.User)
                .WithMany(u => u.EmailVerificationTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");
            entity.HasKey(e => e.TokenId);
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token").HasMaxLength(500).IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.RevokedReason).HasColumnName("revoked_reason").HasMaxLength(500);
            entity.Property(e => e.ReplacedByToken).HasColumnName("replaced_by_token").HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeviceInfo).HasColumnName("device_info").HasMaxLength(500);
            entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);

            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.UserId, e.IsRevoked });

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore computed properties
            entity.Ignore(e => e.IsExpired);
            entity.Ignore(e => e.IsActive);
        });
    }
}
