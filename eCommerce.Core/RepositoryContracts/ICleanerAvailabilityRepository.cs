using eCommerce.Core.DTO.CleanerAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface ICleanerAvailabilityRepository
    {
        Task<CleanerAvailabilityDTO?> GetById(int availabilityId);
        Task<IEnumerable<CleanerAvailabilityDTO>> GetByCleaner(Guid cleanerId, DateTime? from, DateTime? to);
        Task<CleanerAvailabilityDTO> Create(CleanerAvailabilityCreateDTO dto);
        Task<CleanerAvailabilityDTO?> Update(int availabilityId, CleanerAvailabilityUpdateDTO dto);
        Task<bool> Delete(int availabilityId);

        // overlap detection; excludeAvailabilityId used during updates
        Task<bool> HasOverlap(Guid cleanerId, DateTime date, TimeSpan start, TimeSpan end, int? excludeAvailabilityId = null);

    }
}
