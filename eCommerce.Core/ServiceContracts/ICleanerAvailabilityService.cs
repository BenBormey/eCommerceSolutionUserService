using eCommerce.Core.DTO.CleanerAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface ICleanerAvailabilityService
    {
        Task<CleanerAvailabilityDTO?> GetByIdAsync(int availabilityId);
        Task<IEnumerable<CleanerAvailabilityDTO>> GetByCleanerAsync(
            Guid cleanerId, DateTime? from = null, DateTime? to = null);

        Task<CleanerAvailabilityDTO> CreateAsync(CleanerAvailabilityCreateDTO dto);

      
        Task<CleanerAvailabilityDTO?> UpdateAsync(int availabilityId, CleanerAvailabilityUpdateDTO dto);

      
        Task<bool> DeleteAsync(int availabilityId);


        Task<bool> IsCleanerFreeAsync(Guid cleanerId, DateTime date, TimeSpan startTime, TimeSpan endTime);

    }
}
