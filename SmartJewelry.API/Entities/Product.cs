namespace SmartJewelry.API.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty; // finished_jewelry, mounting, loose_gemstone_ref
    public int? CategoryId { get; set; }
    public decimal BasePrice { get; set; }
    public string? MaterialType { get; set; }
    public decimal? GoldWeightGrams { get; set; }
    public string? GoldKarat { get; set; }
    public string? Variants { get; set; } // JSON
    public string? Media360 { get; set; } // JSON
    public string? MountingCompatibility { get; set; } // JSON
    public string? Tags { get; set; } // JSON
    public string? SeoMetadata { get; set; } // JSON
    public string? Gemstones { get; set; } // JSON
    public string? Description { get; set; }
    public decimal? WeightGrams { get; set; }
    public string PublishStatus { get; set; } = "draft";
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Category? Category { get; set; }
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CustomOrderDetail> CustomOrderDetailsAsMounting { get; set; } = new List<CustomOrderDetail>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
