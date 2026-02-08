using System;
using Microsoft.EntityFrameworkCore;
using SmartJewelry.API.Data;
using SmartJewelry.API.DTOs.Orders;
using SmartJewelry.API.Services.Interfaces;

namespace SmartJewelry.API.Services;

public class OrderService : IOrderService
{
    private readonly AiJgsmsFinalContext _db;

    public OrderService(AiJgsmsFinalContext db)
    {
        _db = db;
    }

    public async Task<List<OrderListItemDto>> GetMyOrdersAsync(int userId)
    {
        var salesStaffId = await _db.SalesStaffs
            .Where(s => s.UserId == userId)
            .Select(s => (int?)s.SalesStaffId)
            .FirstOrDefaultAsync();

        if (salesStaffId == null)
        {
            return new List<OrderListItemDto>();
        }

        var orders = await _db.Orders
            .AsNoTracking()
            .Where(o => o.SalesStaffId == salesStaffId)
            .Include(o => o.Customer)
                .ThenInclude(c => c.User)
            .Include(o => o.OrderItems)
            .Include(o => o.Payments)
            .OrderByDescending(o => o.OrderDate ?? o.UpdatedAt)
            .Select(o => new OrderListItemDto
            {
                OrderId = o.OrderId,
                OrderNumber = o.OrderNumber,
                CustomerId = o.CustomerId,
                // Database-first User entity in this project doesn't have FullName.
                // Use Username as a reasonable display name.
                CustomerName = o.Customer.User.Username,
                CustomerEmail = o.Customer.User.Email,
                // Phone is stored on Customer table (Customer.Phone)
                CustomerPhone = o.Customer.Phone,
                OrderType = o.OrderType,
                OrderStatus = o.OrderStatus,
                TotalAmount = o.TotalAmount,
                DiscountAmount = o.DiscountAmount,
                TaxAmount = o.TaxAmount,
                GrandTotal = o.GrandTotal,
                OrderDate = o.OrderDate,
                UpdatedAt = o.UpdatedAt,
                ItemsCount = o.OrderItems.Count,
                LatestPaymentStatus = o.Payments
                    // Payment doesn't have CreatedAt in the DB schema; use PaymentDate, fallback to PaymentId.
                    .OrderByDescending(p => p.PaymentDate ?? DateTime.MinValue)
                    .ThenByDescending(p => p.PaymentId)
                    .Select(p => p.PaymentStatus)
                    .FirstOrDefault(),
                LatestPaymentMethod = o.Payments
                    .OrderByDescending(p => p.PaymentDate ?? DateTime.MinValue)
                    .ThenByDescending(p => p.PaymentId)
                    .Select(p => p.PaymentMethod)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return orders;
    }
}
