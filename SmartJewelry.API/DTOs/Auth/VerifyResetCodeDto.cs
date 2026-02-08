using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.API.DTOs.Auth;

public class VerifyResetCodeDto
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mã xác thực là bắt buộc")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác thực phải có 6 ký tự")]
    public string Code { get; set; } = string.Empty;
}
