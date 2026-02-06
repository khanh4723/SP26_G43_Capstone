using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.API.DTOs.Profile;

public class AddressDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
    public string RecipientName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
    public string AddressLine { get; set; } = string.Empty;
    
    public string? Ward { get; set; }
    
    [Required(ErrorMessage = "Quận/Huyện là bắt buộc")]
    public string District { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc")]
    public string City { get; set; } = string.Empty;
    
    public bool IsDefault { get; set; }
}
