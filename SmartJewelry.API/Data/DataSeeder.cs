using Microsoft.EntityFrameworkCore;
using SmartJewelry.API.Entities;

namespace SmartJewelry.API.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AiJgsmsFinalContext db, ILogger? logger = null)
    {
        var now = DateTime.UtcNow;

        // ===== 1) Categories (check by CategoryName) =====
        var catRings = await db.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Rings");
        if (catRings == null)
        {
            catRings = new Category
            {
                CategoryName = "Rings",
                Description = "Demo category",
                DisplayOrder = 1,
                IsActive = true
            };
            db.Categories.Add(catRings);
            await db.SaveChangesAsync();
        }

        var catNecklaces = await db.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Necklaces");
        if (catNecklaces == null)
        {
            catNecklaces = new Category
            {
                CategoryName = "Necklaces",
                Description = "Demo category",
                DisplayOrder = 2,
                IsActive = true
            };
            db.Categories.Add(catNecklaces);
            await db.SaveChangesAsync();
        }

        // ===== 2) Products (check by ProductCode) =====
        var p1 = await db.Products.FirstOrDefaultAsync(p => p.ProductCode == "PRD-RING-001");
        if (p1 == null)
        {
            p1 = new Product
            {
                ProductCode = "PRD-RING-001",
                ProductName = "Classic Gold Ring",
                ProductType = "finished_jewelry",   // constraint ok
                CategoryId = catRings.CategoryId,
                BasePrice = 3500000m,
                MaterialType = "Gold",
                GoldKarat = "18K",
                WeightGrams = 2.5m,
                PublishStatus = "published",        // constraint ok
                PublishedAt = now,
                CreatedAt = now,
                UpdatedAt = now,
                IsActive = true
            };
            db.Products.Add(p1);
            await db.SaveChangesAsync();
        }

        var p2 = await db.Products.FirstOrDefaultAsync(p => p.ProductCode == "PRD-NECK-001");
        if (p2 == null)
        {
            p2 = new Product
            {
                ProductCode = "PRD-NECK-001",
                ProductName = "Minimal Necklace",
                ProductType = "finished_jewelry",
                CategoryId = catNecklaces.CategoryId,
                BasePrice = 5200000m,
                MaterialType = "Gold",
                GoldKarat = "18K",
                WeightGrams = 3.2m,
                PublishStatus = "published",
                PublishedAt = now,
                CreatedAt = now,
                UpdatedAt = now,
                IsActive = true
            };
            db.Products.Add(p2);
            await db.SaveChangesAsync();
        }

        // ===== 3) Users (check by Email) =====
        var staffUser = await db.Users.FirstOrDefaultAsync(u => u.Email == "salesstaff1@demo.com");
        if (staffUser == null)
        {
            staffUser = new User
            {
                Username = "salesstaff1",
                Email = "salesstaff1@demo.com",
                Role = "sales_staff", // constraint ok
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                IsActive = true,
                EmailVerified = true,
                CreatedAt = now,
                UpdatedAt = now,
                LastLogin = null
            };
            db.Users.Add(staffUser);
            await db.SaveChangesAsync();
        }

        var customerUser = await db.Users.FirstOrDefaultAsync(u => u.Email == "customer1@demo.com");
        if (customerUser == null)
        {
            customerUser = new User
            {
                Username = "customer1",
                Email = "customer1@demo.com",
                Role = "customer",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                IsActive = true,
                EmailVerified = true,
                CreatedAt = now,
                UpdatedAt = now,
                LastLogin = null
            };
            db.Users.Add(customerUser);
            await db.SaveChangesAsync();
        }

        // ===== 4) Role detail rows (check by UserId) =====
        var salesStaff = await db.SalesStaffs.FirstOrDefaultAsync(s => s.UserId == staffUser.UserId);
        if (salesStaff == null)
        {
            salesStaff = new SalesStaff
            {
                UserId = staffUser.UserId,
                DepartmentName = "Sales",
                SalesTarget = 100000000m,
                CommissionRate = 0.02m,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            db.SalesStaffs.Add(salesStaff);
            await db.SaveChangesAsync();
        }

        var customer = await db.Customers.FirstOrDefaultAsync(c => c.UserId == customerUser.UserId);
        if (customer == null)
        {
            customer = new Customer
            {
                UserId = customerUser.UserId,
                Phone = "0900000001",
                LoyaltyPoints = 120,
                CustomerTier = "bronze",
                Gender = "other"
            };
            db.Customers.Add(customer);
            await db.SaveChangesAsync();
        }

        // ===== 5) Orders + Items + Payments (check by OrderNumber / PaymentNumber) =====

        // Order 1
        var order1 = await db.Orders.FirstOrDefaultAsync(o => o.OrderNumber == "ORD-0001");
        if (order1 == null)
        {
            var order1Total = p1.BasePrice * 1;
            var order1Tax = Math.Round(order1Total * 0.1m, 0);
            var order1Discount = 0m;
            var order1Grand = order1Total + order1Tax - order1Discount;

            order1 = new Order
            {
                OrderNumber = "ORD-0001",
                CustomerId = customer.CustomerId,
                SalesStaffId = salesStaff.SalesStaffId,
                OrderType = "retail",
                OrderStatus = "processing",
                TotalAmount = order1Total,
                DiscountAmount = order1Discount,
                TaxAmount = order1Tax,
                GrandTotal = order1Grand,
                OrderDate = now.AddDays(-1),
                UpdatedAt = now.AddHours(-6)
            };
            db.Orders.Add(order1);
            await db.SaveChangesAsync();

            // item
            var hasItem = await db.OrderItems.AnyAsync(i => i.OrderId == order1.OrderId && i.ProductId == p1.ProductId);
            if (!hasItem)
            {
                db.OrderItems.Add(new OrderItem
                {
                    OrderId = order1.OrderId,
                    ProductId = p1.ProductId,
                    Quantity = 1,
                    UnitPrice = p1.BasePrice,
                    Subtotal = p1.BasePrice
                });
            }

            // payment
            var pay1 = await db.Payments.FirstOrDefaultAsync(p => p.PaymentNumber == "PAY-0001");
            if (pay1 == null)
            {
                db.Payments.Add(new Payment
                {
                    PaymentNumber = "PAY-0001",
                    OrderId = order1.OrderId,
                    PaymentType = "full",
                    PaymentAmount = order1Grand,
                    PaymentMethod = "cash",
                    PaymentStatus = "completed",
                    PaymentDate = now.AddHours(-12),
                    TransactionReference = "DEMO-TXN-0001"
                });
            }

            await db.SaveChangesAsync();
        }

        // Order 2
        var order2 = await db.Orders.FirstOrDefaultAsync(o => o.OrderNumber == "ORD-0002");
        if (order2 == null)
        {
            var order2Total = p2.BasePrice * 2;
            var order2Tax = Math.Round(order2Total * 0.1m, 0);
            var order2Discount = 200000m;
            var order2Grand = order2Total + order2Tax - order2Discount;

            order2 = new Order
            {
                OrderNumber = "ORD-0002",
                CustomerId = customer.CustomerId,
                SalesStaffId = salesStaff.SalesStaffId,
                OrderType = "retail",
                OrderStatus = "pending",
                TotalAmount = order2Total,
                DiscountAmount = order2Discount,
                TaxAmount = order2Tax,
                GrandTotal = order2Grand,
                OrderDate = now.AddHours(-8),
                UpdatedAt = now.AddHours(-2)
            };
            db.Orders.Add(order2);
            await db.SaveChangesAsync();

            var hasItem = await db.OrderItems.AnyAsync(i => i.OrderId == order2.OrderId && i.ProductId == p2.ProductId);
            if (!hasItem)
            {
                db.OrderItems.Add(new OrderItem
                {
                    OrderId = order2.OrderId,
                    ProductId = p2.ProductId,
                    Quantity = 2,
                    UnitPrice = p2.BasePrice,
                    Subtotal = p2.BasePrice * 2
                });
            }

            var pay2 = await db.Payments.FirstOrDefaultAsync(p => p.PaymentNumber == "PAY-0002");
            if (pay2 == null)
            {
                db.Payments.Add(new Payment
                {
                    PaymentNumber = "PAY-0002",
                    OrderId = order2.OrderId,
                    PaymentType = "deposit",
                    PaymentAmount = Math.Round(order2Grand * 0.3m, 0),
                    PaymentMethod = "bank_transfer",
                    PaymentStatus = "pending",
                    PaymentDate = now.AddHours(-1),
                    DepositPercentage = 0.3m,
                    TransactionReference = "DEMO-TXN-0002"
                });
            }

            await db.SaveChangesAsync();
        }

        logger?.LogInformation("Seed completed (idempotent). Demo login: salesstaff1@demo.com / 123456");
    }
}
