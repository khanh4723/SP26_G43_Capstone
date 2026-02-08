using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.Web.ViewModels.Profile;

public class UpdateProfileViewModel
{
    [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
    [StringLength(50, ErrorMessage = "Tên người dùng không được quá 50 ký tự")]
    [Display(Name = "Tên người dùng")]
    public string Username { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string? Phone { get; set; }
    
    [Display(Name = "Ngày sinh")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
    
    [Display(Name = "Giới tính")]
    public string? Gender { get; set; }
}
