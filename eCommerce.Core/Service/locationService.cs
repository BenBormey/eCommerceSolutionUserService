using AutoMapper;
using eCommerce.Core.DTO.Location;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repo;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // Read
        public Task<IEnumerable<LocationDTO>> GetAllAsync(Guid userid) => _repo.GetLocations(userid);

        public Task<LocationDTO?> GetByIdAsync(int locationId) => _repo.GetLocationById(locationId);

        // Create
        public Task<LocationDTO> CreateAsync(LocationCreateDTO dto) => _repo.Create(dto);

        // Update
        public Task<LocationDTO?> UpdateAsync(int locationId, LocationUpdateDTO dto)
        {
            // keep route/body ids consistent
            if (dto.LocationId == 0) dto.LocationId = locationId;
            else if (dto.LocationId != locationId)
                throw new ArgumentException("Route id and body LocationId mismatch.");

            return _repo.Update(locationId, dto);
        }

        // Delete
        public Task<bool> DeleteAsync(int locationId) => _repo.Delete(locationId);

        // Extras
        public Task<IEnumerable<LocationDTO>> SearchAsync(string? city = null, string? district = null, string? postalCode = null)
            => _repo.Search(city, district, postalCode);

        public Task<bool> ExistsAsync(int locationId) => _repo.Exists(locationId);
    }
}
