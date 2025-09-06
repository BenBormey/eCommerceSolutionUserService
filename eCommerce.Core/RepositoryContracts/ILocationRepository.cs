using eCommerce.Core.DTO.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface ILocationRepository
    {
        Task<IEnumerable<LocationDTO>> GetLocations();
        Task<LocationDTO?> GetLocationById(int locationId);

        // Create
        Task<LocationDTO> Create(LocationCreateDTO dto);

        // Update
        Task<LocationDTO?> Update(int locationId, LocationUpdateDTO dto);

        // Delete
        Task<bool> Delete(int locationId);

        // Extras
        Task<IEnumerable<LocationDTO>> Search(string? city = null, string? district = null, string? postalCode = null);
        Task<bool> Exists(int locationId);
    }
}
