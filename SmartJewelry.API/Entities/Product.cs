using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductType { get; set; } = null!;

    public int? CategoryId { get; set; }

    public decimal BasePrice { get; set; }

    public string? MaterialType { get; set; }

    public decimal? GoldWeightGrams { get; set; }

    public string? GoldKarat { get; set; }

    public string? Variants { get; set; }

    public string? Media360 { get; set; }

    public string? MountingCompatibility { get; set; }

    public string? Tags { get; set; }

    public string? SeoMetadata { get; set; }

    public string? Gemstones { get; set; }

    public string? Description { get; set; }

    public decimal? WeightGrams { get; set; }

    public string? PublishStatus { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<CustomOrderDetail> CustomOrderDetails { get; set; } = new List<CustomOrderDetail>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
