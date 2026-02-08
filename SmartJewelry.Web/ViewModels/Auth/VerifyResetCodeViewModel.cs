using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.Web.ViewModels.Auth;

public class VerifyResetCodeViewModel
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mã xác nhận là bắt buộc")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác nhận phải có 6 ký tự")]
    [Display(Name = "Mã xác nhận")]
    public string ResetCode { get; set; } = string.Empty;
}
