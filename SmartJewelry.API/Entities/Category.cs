using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartJewelry.API.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? ParentCategoryId { get; set; }

    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    // Convenience alias for UI code.
    // IMPORTANT: mark NotMapped so EF won't treat it as a navigation and create a shadow FK (CategoryId1).
    [NotMapped]
    public ICollection<Category> SubCategories => InverseParentCategory;
}
