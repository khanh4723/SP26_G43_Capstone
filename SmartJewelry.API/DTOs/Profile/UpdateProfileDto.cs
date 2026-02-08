using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.API.DTOs.Profile;

public class UpdateProfileDto
{
    [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
    [StringLength(50, ErrorMessage = "Tên người dùng không được quá 50 ký tự")]
    public string Username { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    [RegularExpression("^(male|female|other)$", ErrorMessage = "Giới tính phải là male, female hoặc other")]
    public string? Gender { get; set; }
}
