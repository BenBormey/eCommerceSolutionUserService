using Dapper;
using eCommerce.Core.DTO.CleanerAvailability;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class CleanerAvailabilityRepository : ICleanerAvailabilityRepository
    {
        private readonly DapperDbContext _db;
        public CleanerAvailabilityRepository(DapperDbContext db) => _db = db;

        // Read one
        public async Task<CleanerAvailabilityDTO?> GetById(int availabilityId)
        {
            const string sql = @"
                SELECT
                    availability_id AS ""AvailabilityId"",
                    cleaner_id      AS ""CleanerId"",
                    available_date  AS ""AvailableDate"",
                    start_time      AS ""StartTime"",
                    end_time        AS ""EndTime""
                FROM public.cleaner_availability
                WHERE availability_id = @Id;";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<CleanerAvailabilityDTO>(sql, new { Id = availabilityId });
        }

        // Read by cleaner (optional date range)
        public async Task<IEnumerable<CleanerAvailabilityDTO>> GetByCleaner(Guid cleanerId, DateTime? from, DateTime? to)
        {
            const string sql = @"
                SELECT
                    availability_id AS ""AvailabilityId"",
                    cleaner_id      AS ""CleanerId"",
                    available_date  AS ""AvailableDate"",
                    start_time      AS ""StartTime"",
                    end_time        AS ""EndTime""
                FROM public.cleaner_availability
                WHERE cleaner_id = @CleanerId
                  AND (@From IS NULL OR available_date >= @From)
                  AND (@To   IS NULL OR available_date <= @To)
                ORDER BY available_date, start_time;";

            return await _db.DbConnection.QueryAsync<CleanerAvailabilityDTO>(sql, new { CleanerId = cleanerId, From = from, To = to });
        }

        // Create
        public async Task<CleanerAvailabilityDTO> Create(CleanerAvailabilityCreateDTO dto)
        {
            const string sql = @"
                INSERT INTO public.cleaner_availability (cleaner_id, available_date, start_time, end_time)
                VALUES (@CleanerId, @AvailableDate, @StartTime, @EndTime)
                RETURNING
                    availability_id AS ""AvailabilityId"",
                    cleaner_id      AS ""CleanerId"",
                    available_date  AS ""AvailableDate"",
                    start_time      AS ""StartTime"",
                    end_time        AS ""EndTime"";";

            return await _db.DbConnection.QuerySingleAsync<CleanerAvailabilityDTO>(sql, new
            {
                dto.CleanerId,
                dto.AvailableDate,
                dto.StartTime,
                dto.EndTime
            });
        }

        // Update
        public async Task<CleanerAvailabilityDTO?> Update(int availabilityId, CleanerAvailabilityUpdateDTO dto)
        {
            const string sql = @"
                UPDATE public.cleaner_availability
                SET
                    cleaner_id     = @CleanerId,
                    available_date = @AvailableDate,
                    start_time     = @StartTime,
                    end_time       = @EndTime
                WHERE availability_id = @Id
                RETURNING
                    availability_id AS ""AvailabilityId"",
                    cleaner_id      AS ""CleanerId"",
                    available_date  AS ""AvailableDate"",
                    start_time      AS ""StartTime"",
                    end_time        AS ""EndTime"";";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<CleanerAvailabilityDTO>(sql, new
            {
                Id = availabilityId,
                dto.CleanerId,
                dto.AvailableDate,
                dto.StartTime,
                dto.EndTime
            });
        }

        // Delete
        public async Task<bool> Delete(int availabilityId)
        {
            const string sql = @"DELETE FROM public.cleaner_availability WHERE availability_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = availabilityId });
            return rows > 0;
        }

        // Overlap detection (same cleaner + same date + time overlap)
        public async Task<bool> HasOverlap(Guid cleanerId, DateTime date, TimeSpan start, TimeSpan end, int? excludeAvailabilityId = null)
        {
            const string sql = @"
                SELECT EXISTS(
                    SELECT 1
                    FROM public.cleaner_availability
                    WHERE cleaner_id = @CleanerId
                      AND available_date = @Date
                      AND NOT (@End <= start_time OR @Start >= end_time)   -- time ranges overlap
                      AND (@ExcludeId IS NULL OR availability_id <> @ExcludeId)
                );";

            return await _db.DbConnection.ExecuteScalarAsync<bool>(sql, new
            {
                CleanerId = cleanerId,
                Date = date,
                Start = start,
                End = end,
                ExcludeId = excludeAvailabilityId
            });
        }
    }
}
