using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartJewelry.API.Data;
using SmartJewelry.API.DTOs.Profile;
using SmartJewelry.API.Entities;
using SmartJewelry.API.Services.Interfaces;

namespace SmartJewelry.API.Services;

public class ProfileService : IProfileService
{
    private readonly SmartJewelryDbContext _context;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(SmartJewelryDbContext context, ILogger<ProfileService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProfileDto?> GetProfileAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Customer)
                    .ThenInclude(c => c!.CustomerProfile)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return null;

            var profileDto = new ProfileDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };

            if (user.Customer != null)
            {
                profileDto.CustomerId = user.Customer.CustomerId;
                profileDto.LoyaltyPoints = user.Customer.LoyaltyPoints;
                profileDto.Phone = user.Customer.Phone;
                profileDto.DateOfBirth = user.Customer.DateOfBirth;
                profileDto.Gender = user.Customer.Gender;
                profileDto.CustomerTier = user.Customer.CustomerTier;

                if (user.Customer.CustomerProfile != null)
                {
                    var profile = user.Customer.CustomerProfile;
                    
                    // Parse addresses from JSON
                    if (!string.IsNullOrEmpty(profile.Addresses))
                    {
                        try
                        {
                            profileDto.Addresses = JsonSerializer.Deserialize<List<AddressDto>>(profile.Addresses) ?? new();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error parsing addresses for user {UserId}", userId);
                            profileDto.Addresses = new();
                        }
                    }

                    // Parse ring sizes from JSON
                    if (!string.IsNullOrEmpty(profile.RingSizes))
                    {
                        try
                        {
                            profileDto.RingSizes = JsonSerializer.Deserialize<List<string>>(profile.RingSizes) ?? new();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error parsing ring sizes for user {UserId}", userId);
                            profileDto.RingSizes = new();
                        }
                    }

                    // Parse preferences from JSON
                    if (!string.IsNullOrEmpty(profile.Preferences))
                    {
                        try
                        {
                            profileDto.Preferences = JsonSerializer.Deserialize<Dictionary<string, string>>(profile.Preferences) ?? new();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error parsing preferences for user {UserId}", userId);
                            profileDto.Preferences = new();
                        }
                    }
                }
            }

            return profileDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profile for user {UserId}", userId);
            return null;
        }
    }

    public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return false;

            // Update user info
            user.Username = updateProfileDto.Username;
            user.UpdatedAt = DateTime.UtcNow;

            // Update customer info if exists
            if (user.Customer != null)
            {
                user.Customer.Phone = updateProfileDto.Phone;
                user.Customer.DateOfBirth = updateProfileDto.DateOfBirth;
                user.Customer.Gender = updateProfileDto.Gender;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> AddAddressAsync(int userId, AddressDto addressDto)
    {
        try
        {
            var customer = await _context.Customers
                .Include(c => c.CustomerProfile)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
                return false;

            // Ensure customer profile exists
            if (customer.CustomerProfile == null)
            {
                customer.CustomerProfile = new CustomerProfile
                {
                    CustomerId = customer.CustomerId,
                    Addresses = "[]",
                    RingSizes = "[]",
                    Preferences = "{}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.CustomerProfiles.Add(customer.CustomerProfile);
                await _context.SaveChangesAsync();
            }

            var profile = customer.CustomerProfile;
            var addresses = string.IsNullOrEmpty(profile.Addresses) 
                ? new List<AddressDto>() 
                : JsonSerializer.Deserialize<List<AddressDto>>(profile.Addresses) ?? new List<AddressDto>();

            // If this is the first address or marked as default, set it as default
            if (addresses.Count == 0 || addressDto.IsDefault)
            {
                // Remove default from other addresses
                foreach (var addr in addresses)
                {
                    addr.IsDefault = false;
                }
                addressDto.IsDefault = true;
            }

            // Generate new ID if not provided
            if (string.IsNullOrEmpty(addressDto.Id))
            {
                addressDto.Id = Guid.NewGuid().ToString();
            }

            addresses.Add(addressDto);
            profile.Addresses = JsonSerializer.Serialize(addresses);
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding address for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> UpdateAddressAsync(int userId, string addressId, AddressDto addressDto)
    {
        try
        {
            var profile = await _context.CustomerProfiles
                .Include(cp => cp.Customer)
                .FirstOrDefaultAsync(cp => cp.Customer.UserId == userId);

            if (profile == null)
                return false;

            var addresses = string.IsNullOrEmpty(profile.Addresses)
                ? new List<AddressDto>()
                : JsonSerializer.Deserialize<List<AddressDto>>(profile.Addresses) ?? new List<AddressDto>();

            var existingAddress = addresses.FirstOrDefault(a => a.Id == addressId);
            if (existingAddress == null)
                return false;

            // If setting as default, remove default from others
            if (addressDto.IsDefault)
            {
                foreach (var addr in addresses)
                {
                    addr.IsDefault = false;
                }
            }

            // Update address
            existingAddress.RecipientName = addressDto.RecipientName;
            existingAddress.Phone = addressDto.Phone;
            existingAddress.AddressLine = addressDto.AddressLine;
            existingAddress.Ward = addressDto.Ward;
            existingAddress.District = addressDto.District;
            existingAddress.City = addressDto.City;
            existingAddress.IsDefault = addressDto.IsDefault;

            profile.Addresses = JsonSerializer.Serialize(addresses);
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating address {AddressId} for user {UserId}", addressId, userId);
            return false;
        }
    }

    public async Task<bool> DeleteAddressAsync(int userId, string addressId)
    {
        try
        {
            var profile = await _context.CustomerProfiles
                .Include(cp => cp.Customer)
                .FirstOrDefaultAsync(cp => cp.Customer.UserId == userId);

            if (profile == null)
                return false;

            var addresses = string.IsNullOrEmpty(profile.Addresses)
                ? new List<AddressDto>()
                : JsonSerializer.Deserialize<List<AddressDto>>(profile.Addresses) ?? new List<AddressDto>();

            var addressToRemove = addresses.FirstOrDefault(a => a.Id == addressId);
            if (addressToRemove == null)
                return false;

            var wasDefault = addressToRemove.IsDefault;
            addresses.Remove(addressToRemove);

            // If removed address was default and there are other addresses, set first one as default
            if (wasDefault && addresses.Count > 0)
            {
                addresses[0].IsDefault = true;
            }

            profile.Addresses = JsonSerializer.Serialize(addresses);
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting address {AddressId} for user {UserId}", addressId, userId);
            return false;
        }
    }

    public async Task<bool> SetDefaultAddressAsync(int userId, string addressId)
    {
        try
        {
            var profile = await _context.CustomerProfiles
                .Include(cp => cp.Customer)
                .FirstOrDefaultAsync(cp => cp.Customer.UserId == userId);

            if (profile == null)
                return false;

            var addresses = string.IsNullOrEmpty(profile.Addresses)
                ? new List<AddressDto>()
                : JsonSerializer.Deserialize<List<AddressDto>>(profile.Addresses) ?? new List<AddressDto>();

            var targetAddress = addresses.FirstOrDefault(a => a.Id == addressId);
            if (targetAddress == null)
                return false;

            // Remove default from all addresses
            foreach (var addr in addresses)
            {
                addr.IsDefault = false;
            }

            // Set target as default
            targetAddress.IsDefault = true;

            profile.Addresses = JsonSerializer.Serialize(addresses);
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting default address {AddressId} for user {UserId}", addressId, userId);
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                return false;
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user {UserId}", userId);
            return false;
        }
    }
}
