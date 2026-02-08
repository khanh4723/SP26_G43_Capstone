using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartJewelry.API.DTOs.Common;
using SmartJewelry.API.DTOs.Orders;
using SmartJewelry.API.Entities;
using SmartJewelry.API.Services.Interfaces;
using System.Security.Claims;

namespace SmartJewelry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// SalesStaff: xem danh sách order của chính mình (Order.SalesStaffId = SalesStaffId của user)
    /// </summary>
    [HttpGet("my")]
    [Authorize(Roles = "sales_staff")]
    [ProducesResponseType(typeof(ApiResponse<List<OrderListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyOrders()
    {
        var userIdClaim = User.FindFirstValue("userId")
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(ApiResponse<List<OrderListItemDto>>.FailResponse("Token không hợp lệ"));
        }

        var data = await _orderService.GetMyOrdersAsync(userId);
        return Ok(ApiResponse<List<OrderListItemDto>>.SuccessResponse(data, "Lấy danh sách đơn hàng thành công"));
    }
}
