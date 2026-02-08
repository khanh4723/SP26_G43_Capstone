using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.Web.ViewModels.Profile;

public class AddressViewModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
    [Display(Name = "Tên người nhận")]
    public string RecipientName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
    [Display(Name = "Địa chỉ chi tiết")]
    public string AddressLine { get; set; } = string.Empty;
    
    [Display(Name = "Phường/Xã")]
    public string? Ward { get; set; }
    
    [Required(ErrorMessage = "Quận/Huyện là bắt buộc")]
    [Display(Name = "Quận/Huyện")]
    public string District { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc")]
    [Display(Name = "Tỉnh/Thành phố")]
    public string City { get; set; } = string.Empty;
    
    [Display(Name = "Địa chỉ mặc định")]
    public bool IsDefault { get; set; }
}
