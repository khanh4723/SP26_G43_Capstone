using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartJewelry.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    parent_category_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.category_id);
                    table.ForeignKey(
                        name: "FK_Category_Category_parent_category_id",
                        column: x => x.parent_category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "Gemstone",
                columns: table => new
                {
                    gemstone_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gemstone_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gemstone_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    weight_carats = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: true),
                    shape = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    clarity_grade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    color_grade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cut_grade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    treatment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    origin_country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fluorescence = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    certificate_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    certificate_lab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cert_file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    image_360_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    video_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    purchase_price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
                    selling_price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
                    markup_percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    gemstone_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reserved_until = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gemstone", x => x.gemstone_id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    social_login_provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    social_login_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    email_verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    product_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    base_price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    material_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    gold_weight_grams = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: true),
                    gold_karat = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    variants = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    media_360 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mounting_compatibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seo_metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gemstones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    weight_grams = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    publish_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    published_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_Product_Category_category_id",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    entity_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    entity_id = table.Column<int>(type: "int", nullable: true),
                    action_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    action_timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    old_values_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    new_values_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_ActivityLog_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    permission_level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.admin_id);
                    table.ForeignKey(
                        name: "FK_Admin_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentCreator",
                columns: table => new
                {
                    content_creator_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    specialty_area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    content_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCreator", x => x.content_creator_id);
                    table.ForeignKey(
                        name: "FK_ContentCreator_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    loyalty_points = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    customer_tier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_Customer_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerificationToken",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    used_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationToken", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_EmailVerificationToken_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryManager",
                columns: table => new
                {
                    inventory_manager_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    certification_level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryManager", x => x.inventory_manager_id);
                    table.ForeignKey(
                        name: "FK_InventoryManager_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetToken",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    used_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetToken", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_PasswordResetToken_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_revoked = table.Column<bool>(type: "bit", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    revoked_reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    replaced_by_token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    device_info = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesStaff",
                columns: table => new
                {
                    sales_staff_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    department_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sales_target = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
                    commission_rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    hire_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesStaff", x => x.sales_staff_id);
                    table.ForeignKey(
                        name: "FK_SalesStaff_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreManager",
                columns: table => new
                {
                    store_manager_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    managed_department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    supervised_staff_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreManager", x => x.store_manager_id);
                    table.ForeignKey(
                        name: "FK_StoreManager_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    MinimumStockThreshold = table.Column<int>(type: "int", nullable: false),
                    WarehouseLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockedSince = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_Inventory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityChange = table.Column<int>(type: "int", nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_InventoryTransaction_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransaction_User_StaffId",
                        column: x => x.StaffId,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    image_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    alt_text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    uploaded_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product_product_id",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoldRate",
                columns: table => new
                {
                    RateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoldType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatePerGram = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorAdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoldRate", x => x.RateId);
                    table.ForeignKey(
                        name: "FK_GoldRate_Admin_CreatorAdminId",
                        column: x => x.CreatorAdminId,
                        principalTable: "Admin",
                        principalColumn: "admin_id");
                });

            migrationBuilder.CreateTable(
                name: "SystemConfig",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdaterAdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfig", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_SystemConfig_Admin_UpdaterAdminId",
                        column: x => x.UpdaterAdminId,
                        principalTable: "Admin",
                        principalColumn: "admin_id");
                });

            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    collection_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collection_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    collection_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    banner_image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    products = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.collection_id);
                    table.ForeignKey(
                        name: "FK_Collection_ContentCreator_created_by",
                        column: x => x.created_by,
                        principalTable: "ContentCreator",
                        principalColumn: "content_creator_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    PublicationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Excerpt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeaturedImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeoMetadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_Content_ContentCreator_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "ContentCreator",
                        principalColumn: "content_creator_id");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    recipient_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    address_line = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ward_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    district = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    district_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    city_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_Address_Customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProfile",
                columns: table => new
                {
                    profile_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    ring_sizes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    addresses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    preferences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vouchers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfile", x => x.profile_id);
                    table.ForeignKey(
                        name: "FK_CustomerProfile_Customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyTransaction",
                columns: table => new
                {
                    transaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    transaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    points_change = table.Column<int>(type: "int", nullable: false),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyTransaction", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransaction_Customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsultationTicket",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SalesStaffId = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AutoAssigned = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationTicket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_ConsultationTicket_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultationTicket_SalesStaff_SalesStaffId",
                        column: x => x.SalesStaffId,
                        principalTable: "SalesStaff",
                        principalColumn: "sales_staff_id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SalesStaffId = table.Column<int>(type: "int", nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PromotionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_SalesStaff_SalesStaffId",
                        column: x => x.SalesStaffId,
                        principalTable: "SalesStaff",
                        principalColumn: "sales_staff_id");
                });

            migrationBuilder.CreateTable(
                name: "SalesConfig",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesStaffId = table.Column<int>(type: "int", nullable: false),
                    Specialties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxActiveTickets = table.Column<int>(type: "int", nullable: false),
                    CurrentActiveTickets = table.Column<int>(type: "int", nullable: false),
                    ShiftSchedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastOnlineAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PerformanceKpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesConfig", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_SalesConfig_SalesStaff_SalesStaffId",
                        column: x => x.SalesStaffId,
                        principalTable: "SalesStaff",
                        principalColumn: "sales_staff_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApplicableProducts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsageLimitPerCustomer = table.Column<int>(type: "int", nullable: false),
                    TotalUsageLimit = table.Column<int>(type: "int", nullable: true),
                    CurrentUsageCount = table.Column<int>(type: "int", nullable: false),
                    UsageHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.PromotionId);
                    table.ForeignKey(
                        name: "FK_Promotion_ContentCreator_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ContentCreator",
                        principalColumn: "content_creator_id");
                    table.ForeignKey(
                        name: "FK_Promotion_StoreManager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "StoreManager",
                        principalColumn: "store_manager_id");
                });

            migrationBuilder.CreateTable(
                name: "PublishRequest",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryManagerId = table.Column<int>(type: "int", nullable: false),
                    StoreManagerId = table.Column<int>(type: "int", nullable: true),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewerNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishRequest", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_PublishRequest_InventoryManager_InventoryManagerId",
                        column: x => x.InventoryManagerId,
                        principalTable: "InventoryManager",
                        principalColumn: "inventory_manager_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublishRequest_StoreManager_StoreManagerId",
                        column: x => x.StoreManagerId,
                        principalTable: "StoreManager",
                        principalColumn: "store_manager_id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    StoreManagerId = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_StoreManager_StoreManagerId",
                        column: x => x.StoreManagerId,
                        principalTable: "StoreManager",
                        principalColumn: "store_manager_id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsultationAudio",
                columns: table => new
                {
                    AudioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    AudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioDurationSeconds = table.Column<int>(type: "int", nullable: true),
                    UploadedBy = table.Column<int>(type: "int", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Transcript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranscriptionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranscribedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Extraction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtractionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtractedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBySales = table.Column<bool>(type: "bit", nullable: false),
                    SalesConfirmedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UploaderSalesStaffId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationAudio", x => x.AudioId);
                    table.ForeignKey(
                        name: "FK_ConsultationAudio_ConsultationTicket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "ConsultationTicket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultationAudio_SalesStaff_UploaderSalesStaffId",
                        column: x => x.UploaderSalesStaffId,
                        principalTable: "SalesStaff",
                        principalColumn: "sales_staff_id");
                });

            migrationBuilder.CreateTable(
                name: "CustomOrderDetail",
                columns: table => new
                {
                    CustomDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ConsultationTicketId = table.Column<int>(type: "int", nullable: true),
                    SelectedGemstoneId = table.Column<int>(type: "int", nullable: true),
                    SelectedMountingId = table.Column<int>(type: "int", nullable: true),
                    Modifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowStages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomOrderDetail", x => x.CustomDetailId);
                    table.ForeignKey(
                        name: "FK_CustomOrderDetail_ConsultationTicket_ConsultationTicketId",
                        column: x => x.ConsultationTicketId,
                        principalTable: "ConsultationTicket",
                        principalColumn: "TicketId");
                    table.ForeignKey(
                        name: "FK_CustomOrderDetail_Gemstone_SelectedGemstoneId",
                        column: x => x.SelectedGemstoneId,
                        principalTable: "Gemstone",
                        principalColumn: "gemstone_id");
                    table.ForeignKey(
                        name: "FK_CustomOrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomOrderDetail_Product_SelectedMountingId",
                        column: x => x.SelectedMountingId,
                        principalTable: "Product",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    VariantSku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GatewayResponse = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    RatingScore = table.Column<int>(type: "int", nullable: false),
                    ReviewTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewImages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HelpfulVotes = table.Column<int>(type: "int", nullable: false),
                    UnhelpfulVotes = table.Column<int>(type: "int", nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false),
                    ReviewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true),
                    ReviewTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewerUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_Review_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_User_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptNote",
                columns: table => new
                {
                    GrnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrnNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    InventoryManagerId = table.Column<int>(type: "int", nullable: true),
                    ReceiptStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptNote", x => x.GrnId);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNote_InventoryManager_InventoryManagerId",
                        column: x => x.InventoryManagerId,
                        principalTable: "InventoryManager",
                        principalColumn: "inventory_manager_id");
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNote_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLine",
                columns: table => new
                {
                    PoLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    LineType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    NewItemSpec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredChecklist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLine", x => x.PoLineId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "product_id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_user_id",
                table: "ActivityLog",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Address_customer_id",
                table: "Address",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_user_id",
                table: "Admin",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_CustomerId",
                table: "Cart",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_parent_category_id",
                table: "Category",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_created_by",
                table: "Collection",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationAudio_TicketId",
                table: "ConsultationAudio",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationAudio_UploaderSalesStaffId",
                table: "ConsultationAudio",
                column: "UploaderSalesStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationTicket_CustomerId",
                table: "ConsultationTicket",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationTicket_SalesStaffId",
                table: "ConsultationTicket",
                column: "SalesStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_AuthorId",
                table: "Content",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCreator_user_id",
                table: "ContentCreator",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_user_id",
                table: "Customer",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_customer_id",
                table: "CustomerProfile",
                column: "customer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomOrderDetail_ConsultationTicketId",
                table: "CustomOrderDetail",
                column: "ConsultationTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomOrderDetail_OrderId",
                table: "CustomOrderDetail",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomOrderDetail_SelectedGemstoneId",
                table: "CustomOrderDetail",
                column: "SelectedGemstoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomOrderDetail_SelectedMountingId",
                table: "CustomOrderDetail",
                column: "SelectedMountingId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationToken_expires_at",
                table: "EmailVerificationToken",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationToken_token",
                table: "EmailVerificationToken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationToken_user_id_is_used",
                table: "EmailVerificationToken",
                columns: new[] { "user_id", "is_used" });

            migrationBuilder.CreateIndex(
                name: "IX_Gemstone_gemstone_code",
                table: "Gemstone",
                column: "gemstone_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gemstone_gemstone_status",
                table: "Gemstone",
                column: "gemstone_status");

            migrationBuilder.CreateIndex(
                name: "IX_Gemstone_gemstone_type",
                table: "Gemstone",
                column: "gemstone_type");

            migrationBuilder.CreateIndex(
                name: "IX_GoldRate_CreatorAdminId",
                table: "GoldRate",
                column: "CreatorAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNote_InventoryManagerId",
                table: "GoodsReceiptNote",
                column: "InventoryManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNote_PurchaseOrderId",
                table: "GoodsReceiptNote",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryManager_user_id",
                table: "InventoryManager",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_ProductId",
                table: "InventoryTransaction",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_StaffId",
                table: "InventoryTransaction",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransaction_customer_id",
                table: "LoyaltyTransaction",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SalesStaffId",
                table: "Order",
                column: "SalesStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_expires_at",
                table: "PasswordResetToken",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_token",
                table: "PasswordResetToken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_user_id_is_used",
                table: "PasswordResetToken",
                columns: new[] { "user_id", "is_used" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_code",
                table: "Product",
                column: "product_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_type",
                table: "Product",
                column: "product_type");

            migrationBuilder.CreateIndex(
                name: "IX_Product_publish_status",
                table: "Product",
                column: "publish_status");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_product_id",
                table: "ProductImage",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_CreatorId",
                table: "Promotion",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ManagerId",
                table: "Promotion",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishRequest_InventoryManagerId",
                table: "PublishRequest",
                column: "InventoryManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishRequest_StoreManagerId",
                table: "PublishRequest",
                column: "StoreManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_StoreManagerId",
                table: "PurchaseOrder",
                column: "StoreManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_SupplierId",
                table: "PurchaseOrder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_ProductId",
                table: "PurchaseOrderLine",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_PurchaseOrderId",
                table: "PurchaseOrderLine",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_expires_at",
                table: "RefreshToken",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_token",
                table: "RefreshToken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_user_id_is_revoked",
                table: "RefreshToken",
                columns: new[] { "user_id", "is_revoked" });

            migrationBuilder.CreateIndex(
                name: "IX_Review_CustomerId",
                table: "Review",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderId",
                table: "Review",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProductId",
                table: "Review",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReviewerUserId",
                table: "Review",
                column: "ReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesConfig_SalesStaffId",
                table: "SalesConfig",
                column: "SalesStaffId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesStaff_user_id",
                table: "SalesStaff",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreManager_user_id",
                table: "StoreManager",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfig_UpdaterAdminId",
                table: "SystemConfig",
                column: "UpdaterAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_role",
                table: "User",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "IX_User_username",
                table: "User",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropTable(
                name: "ConsultationAudio");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "CustomerProfile");

            migrationBuilder.DropTable(
                name: "CustomOrderDetail");

            migrationBuilder.DropTable(
                name: "EmailVerificationToken");

            migrationBuilder.DropTable(
                name: "GoldRate");

            migrationBuilder.DropTable(
                name: "GoodsReceiptNote");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryTransaction");

            migrationBuilder.DropTable(
                name: "LoyaltyTransaction");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "PasswordResetToken");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "PublishRequest");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLine");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "SalesConfig");

            migrationBuilder.DropTable(
                name: "SystemConfig");

            migrationBuilder.DropTable(
                name: "ConsultationTicket");

            migrationBuilder.DropTable(
                name: "Gemstone");

            migrationBuilder.DropTable(
                name: "ContentCreator");

            migrationBuilder.DropTable(
                name: "InventoryManager");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "StoreManager");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "SalesStaff");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
