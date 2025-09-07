using eCommerce.Core.DTO.CleanerAvailability;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class CleanerAvailabilityService : ICleanerAvailabilityService
    {
        private readonly ICleanerAvailabilityRepository _repo;

        public CleanerAvailabilityService(ICleanerAvailabilityRepository repo)
        {
            _repo = repo;
        }

        // READ
        public Task<CleanerAvailabilityDTO?> GetByIdAsync(int availabilityId)
            => _repo.GetById(availabilityId);

        public Task<IEnumerable<CleanerAvailabilityDTO>> GetByCleanerAsync(
            Guid cleanerId, DateTime? from = null, DateTime? to = null)
            => _repo.GetByCleaner(cleanerId, from, to);

        // CREATE
        public async Task<CleanerAvailabilityDTO> CreateAsync(CleanerAvailabilityCreateDTO dto)
        {
            ValidateTimes(dto.StartTime, dto.EndTime);

            // prevent overlaps
            if (await _repo.HasOverlap(dto.CleanerId, dto.AvailableDate, dto.StartTime, dto.EndTime))
                throw new InvalidOperationException("Availability overlaps with an existing slot.");

            return await _repo.Create(dto);
        }

        // UPDATE
        public async Task<CleanerAvailabilityDTO?> UpdateAsync(int availabilityId, CleanerAvailabilityUpdateDTO dto)
        {
            if (dto.AvailabilityId == 0) dto.AvailabilityId = availabilityId;
            else if (dto.AvailabilityId != availabilityId)
                throw new ArgumentException("Route id and body AvailabilityId mismatch.");

            ValidateTimes(dto.StartTime, dto.EndTime);

            if (await _repo.HasOverlap(dto.CleanerId, dto.AvailableDate, dto.StartTime, dto.EndTime, excludeAvailabilityId: availabilityId))
                throw new InvalidOperationException("Availability overlaps with an existing slot.");

            return await _repo.Update(availabilityId, dto);
        }

        // DELETE
        public Task<bool> DeleteAsync(int availabilityId)
            => _repo.Delete(availabilityId);

        // UTILITY
        public async Task<bool> IsCleanerFreeAsync(Guid cleanerId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            if (!IsValidRange(startTime, endTime)) return false;
            return !await _repo.HasOverlap(cleanerId, date, startTime, endTime);
        }

        private static void ValidateTimes(TimeSpan start, TimeSpan end)
        {
            if (!IsValidRange(start, end))
                throw new ArgumentException("EndTime must be after StartTime.");
        }

        private static bool IsValidRange(TimeSpan start, TimeSpan end) => end > start;
    }
}
