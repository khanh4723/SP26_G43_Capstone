using SmartJewelry.API.DTOs.Profile;

namespace SmartJewelry.API.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileDto?> GetProfileAsync(int userId);
    Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto);
    Task<bool> AddAddressAsync(int userId, AddressDto addressDto);
    Task<bool> UpdateAddressAsync(int userId, string addressId, AddressDto addressDto);
    Task<bool> DeleteAddressAsync(int userId, string addressId);
    Task<bool> SetDefaultAddressAsync(int userId, string addressId);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
}
