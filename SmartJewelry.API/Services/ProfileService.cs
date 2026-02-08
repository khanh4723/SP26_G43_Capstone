using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartJewelry.API.Data;
using SmartJewelry.API.DTOs.Profile;
using SmartJewelry.API.Entities;
using SmartJewelry.API.Services.Interfaces;

namespace SmartJewelry.API.Services;

public class ProfileService : IProfileService
{
    private readonly AiJgsmsFinalContext _context;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(AiJgsmsFinalContext context, ILogger<ProfileService> logger)
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
                CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
                LastLogin = user.LastLogin
            };

            if (user.Customer != null)
            {
                profileDto.CustomerId = user.Customer.CustomerId;
                profileDto.LoyaltyPoints = user.Customer.LoyaltyPoints ?? 0;
                profileDto.Phone = user.Customer.Phone;
                profileDto.DateOfBirth = user.Customer.DateOfBirth.HasValue ? user.Customer.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : null;
                profileDto.Gender = user.Customer.Gender;
                profileDto.CustomerTier = user.Customer.CustomerTier;

                // Load addresses from Address table
                var addresses = await _context.Addresses
                    .Where(a => a.CustomerId == user.Customer.CustomerId)
                    .OrderByDescending(a => a.IsDefault)
                    .ThenByDescending(a => a.CreatedAt)
                    .ToListAsync();

                profileDto.Addresses = addresses.Select(a => new AddressDto
                {
                    Id = a.AddressId.ToString(),
                    RecipientName = a.RecipientName,
                    Phone = a.Phone,
                    AddressLine = a.AddressLine,
                    Ward = a.Ward,
                    WardCode = a.WardCode,
                    District = a.District,
                    DistrictCode = a.DistrictCode,
                    City = a.City,
                    CityCode = a.CityCode,
                    IsDefault = a.IsDefault
                }).ToList();

                if (user.Customer.CustomerProfile != null)
                {
                    var profile = user.Customer.CustomerProfile;

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
                user.Customer.DateOfBirth = updateProfileDto.DateOfBirth.HasValue ? DateOnly.FromDateTime(updateProfileDto.DateOfBirth.Value) : null;
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
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
            {
                _logger.LogWarning("Customer not found for user {UserId}", userId);
                return false;
            }

            // If this is marked as default or first address, unset other defaults
            if (addressDto.IsDefault)
            {
                var existingAddresses = await _context.Addresses
                    .Where(a => a.CustomerId == customer.CustomerId && a.IsDefault)
                    .ToListAsync();
                    
                foreach (var addr in existingAddresses)
                {
                    addr.IsDefault = false;
                }
            }
            else
            {
                // If no default exists, make this one default
                var hasDefault = await _context.Addresses
                    .AnyAsync(a => a.CustomerId == customer.CustomerId && a.IsDefault);
                    
                if (!hasDefault)
                {
                    addressDto.IsDefault = true;
                }
            }

            // Create new Address entity
            var address = new Address
            {
                CustomerId = customer.CustomerId,
                RecipientName = addressDto.RecipientName,
                Phone = addressDto.Phone,
                AddressLine = addressDto.AddressLine,
                Ward = addressDto.Ward,
                WardCode = addressDto.WardCode,
                District = addressDto.District,
                DistrictCode = addressDto.DistrictCode,
                City = addressDto.City,
                CityCode = addressDto.CityCode,
                IsDefault = addressDto.IsDefault,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Address added successfully for user {UserId}, AddressId: {AddressId}", userId, address.AddressId);
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
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
            {
                _logger.LogWarning("Customer not found for user {UserId}", userId);
                return false;
            }

            // Parse addressId to int
            if (!int.TryParse(addressId, out var addressIdInt))
            {
                _logger.LogWarning("Invalid addressId format: {AddressId}", addressId);
                return false;
            }

            // Find the address in Address table
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressIdInt && a.CustomerId == customer.CustomerId);

            if (address == null)
            {
                _logger.LogWarning("Address {AddressId} not found for customer {CustomerId}", addressId, customer.CustomerId);
                return false;
            }

            // If setting as default, unset other defaults
            if (addressDto.IsDefault)
            {
                var otherAddresses = await _context.Addresses
                    .Where(a => a.CustomerId == customer.CustomerId && a.AddressId != addressIdInt && a.IsDefault)
                    .ToListAsync();
                    
                foreach (var addr in otherAddresses)
                {
                    addr.IsDefault = false;
                }
            }

            // Update address fields
            address.RecipientName = addressDto.RecipientName;
            address.Phone = addressDto.Phone;
            address.AddressLine = addressDto.AddressLine;
            address.Ward = addressDto.Ward;
            address.WardCode = addressDto.WardCode;
            address.District = addressDto.District;
            address.DistrictCode = addressDto.DistrictCode;
            address.City = addressDto.City;
            address.CityCode = addressDto.CityCode;
            address.IsDefault = addressDto.IsDefault;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Address {AddressId} updated successfully for user {UserId}", addressId, userId);
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
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
            {
                _logger.LogWarning("Customer not found for user {UserId}", userId);
                return false;
            }

            // Parse addressId to int
            if (!int.TryParse(addressId, out var addressIdInt))
            {
                _logger.LogWarning("Invalid addressId format: {AddressId}", addressId);
                return false;
            }

            // Find the address in Address table
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressIdInt && a.CustomerId == customer.CustomerId);

            if (address == null)
            {
                _logger.LogWarning("Address {AddressId} not found for customer {CustomerId}", addressId, customer.CustomerId);
                return false;
            }

            var wasDefault = address.IsDefault;
            
            // Remove the address
            _context.Addresses.Remove(address);

            // If removed address was default, set another one as default
            if (wasDefault)
            {
                var firstOtherAddress = await _context.Addresses
                    .Where(a => a.CustomerId == customer.CustomerId && a.AddressId != addressIdInt)
                    .OrderByDescending(a => a.CreatedAt)
                    .FirstOrDefaultAsync();
                    
                if (firstOtherAddress != null)
                {
                    firstOtherAddress.IsDefault = true;
                }
            }

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Address {AddressId} deleted successfully for user {UserId}", addressId, userId);
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
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
            {
                _logger.LogWarning("Customer not found for user {UserId}", userId);
                return false;
            }

            // Parse addressId to int
            if (!int.TryParse(addressId, out var addressIdInt))
            {
                _logger.LogWarning("Invalid addressId format: {AddressId}", addressId);
                return false;
            }

            // Find the address in Address table
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressIdInt && a.CustomerId == customer.CustomerId);

            if (address == null)
            {
                _logger.LogWarning("Address {AddressId} not found for customer {CustomerId}", addressId, customer.CustomerId);
                return false;
            }

            // Unset default from all other addresses
            var otherAddresses = await _context.Addresses
                .Where(a => a.CustomerId == customer.CustomerId && a.AddressId != addressIdInt && a.IsDefault)
                .ToListAsync();
                
            foreach (var addr in otherAddresses)
            {
                addr.IsDefault = false;
            }

            // Set target as default
            address.IsDefault = true;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Address {AddressId} set as default for user {UserId}", addressId, userId);
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
