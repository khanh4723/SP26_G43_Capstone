using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartJewelry.API.DTOs.Common;
using SmartJewelry.API.DTOs.Profile;
using SmartJewelry.API.Services.Interfaces;
using System.Security.Claims;

namespace SmartJewelry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
    {
        _profileService = profileService;
        _logger = logger;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    // GET: api/Profile
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var profile = await _profileService.GetProfileAsync(userId);
        if (profile == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Không tìm thấy thông tin người dùng"
            });

        return Ok(new ApiResponse<ProfileDto>
        {
            Success = true,
            Message = "Lấy thông tin hồ sơ thành công",
            Data = profile
        });
    }

    // PUT: api/Profile
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ",
                Data = ModelState
            });

        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var result = await _profileService.UpdateProfileAsync(userId, updateProfileDto);
        if (!result)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Cập nhật thông tin thất bại"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cập nhật thông tin thành công"
        });
    }

    // POST: api/Profile/addresses
    [HttpPost("addresses")]
    public async Task<IActionResult> AddAddress([FromBody] AddressDto addressDto)
    {
        _logger.LogInformation("AddAddress called with data: {@AddressDto}", addressDto);
        _logger.LogInformation("ModelState.IsValid: {IsValid}", ModelState.IsValid);
        
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var errorMessage = errors.Any() ? string.Join(", ", errors) : "Dữ liệu không hợp lệ";
            
            _logger.LogWarning("AddAddress validation failed: {Errors}", errorMessage);
            
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = errorMessage
            });
        }

        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var result = await _profileService.AddAddressAsync(userId, addressDto);
        if (!result)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Thêm địa chỉ thất bại"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Thêm địa chỉ thành công"
        });
    }

    // PUT: api/Profile/addresses/{addressId}
    [HttpPut("addresses/{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, [FromBody] AddressDto addressDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ",
                Data = ModelState
            });

        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var result = await _profileService.UpdateAddressAsync(userId, addressId, addressDto);
        if (!result)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Cập nhật địa chỉ thất bại"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Cập nhật địa chỉ thành công"
        });
    }

    // DELETE: api/Profile/addresses/{addressId}
    [HttpDelete("addresses/{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var result = await _profileService.DeleteAddressAsync(userId, addressId);
        if (!result)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Xóa địa chỉ thất bại"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Xóa địa chỉ thành công"
        });
    }

    // PUT: api/Profile/addresses/{addressId}/set-default
    [HttpPut("addresses/{addressId}/set-default")]
    public async Task<IActionResult> SetDefaultAddress(string addressId)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var result = await _profileService.SetDefaultAddressAsync(userId, addressId);
        if (!result)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Đặt địa chỉ mặc định thất bại"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Đặt địa chỉ mặc định thành công"
        });
    }

    // POST: api/Profile/change-password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ",
                Data = ModelState
            });

        var userId = GetCurrentUserId();
        if (userId == 0)
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Không thể xác định người dùng"
            });

        var result = await _profileService.ChangePasswordAsync(userId, changePasswordDto);
        if (!result)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Đổi mật khẩu thất bại. Vui lòng kiểm tra lại mật khẩu hiện tại"
            });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Đổi mật khẩu thành công"
        });
    }
}
