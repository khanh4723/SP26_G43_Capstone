using SmartJewelry.API.DTOs.Orders;

namespace SmartJewelry.API.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderListItemDto>> GetMyOrdersAsync(int userId);
}
