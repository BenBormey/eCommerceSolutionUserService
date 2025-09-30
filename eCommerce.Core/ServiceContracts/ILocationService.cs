using eCommerce.Core.DTO.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationDTO>> GetAllAsync(Guid userId);
        Task<LocationDTO?> GetByIdAsync(int locationId);

        // Create
        Task<LocationDTO> CreateAsync(LocationCreateDTO dto);

        // Update
        Task<LocationDTO?> UpdateAsync(int locationId, LocationUpdateDTO dto);

        // Delete
        Task<bool> DeleteAsync(int locationId);

        // Extras
        Task<IEnumerable<LocationDTO>> SearchAsync(string? city = null, string? district = null, string? postalCode = null);
        Task<bool> ExistsAsync(int locationId);
    }
}
