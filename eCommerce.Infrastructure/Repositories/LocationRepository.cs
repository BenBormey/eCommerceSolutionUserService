using Dapper;
using eCommerce.Core.DTO.Location;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;   // DapperDbContext
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DapperDbContext _db;

        public LocationRepository(DapperDbContext db) => _db = db;

        // Read all
        public async Task<IEnumerable<LocationDTO>> GetLocations(Guid userId )
        {
             string sql = $@"
                           SELECT 
                    l.location_id   AS ""LocationId"",
                    l.city          AS ""City"",
                    l.district      AS  ""District"",
                    l.postal_code   AS ""PostalCode"",
                    l.created_at    AS ""CreatedAt"",
s.user_id, s.full_name
                FROM public.locations as l inner join public.users as s on
				s.user_id  = l.user_id where l.user_id  = '{userId}'
                ORDER BY l.created_at DESC;";

            var conn = _db.DbConnection;
            return await conn.QueryAsync<LocationDTO>(sql);
        }

        // Read by id
        public async Task<LocationDTO?> GetLocationById(int locationId)
        {
            const string sql = @"
            SELECT 
                    l.location_id   AS ""LocationId"",
                    l.city          AS ""City"",
                    l.district      AS  ""District"",
                    l.postal_code   AS ""PostalCode"",
                    l.created_at    AS ""CreatedAt"",
s.user_id, s.full_name
                FROM public.locations
                WHERE location_id = @LocationId;";

            var conn = _db.DbConnection;
            return await conn.QueryFirstOrDefaultAsync<LocationDTO>(sql, new { LocationId = locationId });
        }

        // Create
        public async Task<LocationDTO> Create(LocationCreateDTO dto)
        {
            const string sql = @"
                INSERT INTO public.locations (city, district, postal_code, created_at,user_id)
                VALUES (@City, @District, @PostalCode, NOW(),@user_id)
                RETURNING 
                    location_id   AS ""LocationId"",
                    city          AS ""City"",
                    district      AS ""District"",
                    postal_code   AS ""PostalCode"",
                    created_at    AS ""CreatedAt"",
                    user_id;";

            var conn = _db.DbConnection;
            return await conn.QuerySingleAsync<LocationDTO>(sql, dto);
        }

        // Update
        public async Task<LocationDTO?> Update(int locationId, LocationUpdateDTO dto)
        {
            const string sql = @"
                UPDATE public.locations
                SET 
                    city        = @City,
                    district    = @District,
                    postal_code = @PostalCode
                WHERE location_id = @LocationId
                RETURNING 
                    location_id   AS ""LocationId"",
                    city          AS ""City"",
                    district      AS ""District"",
                    postal_code   AS ""PostalCode"",
                    created_at    AS ""CreatedAt"";";

            var conn = _db.DbConnection;
            return await conn.QueryFirstOrDefaultAsync<LocationDTO>(sql, new
            {
                LocationId = locationId,
                dto.City,
                dto.District,
                dto.PostalCode
            });
        }

        // Delete
        public async Task<bool> Delete(int locationId)
        {
            const string sql = @"DELETE FROM public.locations WHERE location_id = @LocationId;";
            var conn = _db.DbConnection;
            var rows = await conn.ExecuteAsync(sql, new { LocationId = locationId });
            return rows > 0;
        }

        // Search
        public async Task<IEnumerable<LocationDTO>> Search(string? city = null, string? district = null, string? postalCode = null)
        {
            const string sql = @"
                   SELECT 
                    l.location_id   AS ""LocationId"",
                    l.city          AS ""City"",
                    l.district      AS  ""District"",
                    l.postal_code   AS ""PostalCode"",
                    l.created_at    AS ""CreatedAt"",
s.user_id, s.full_name
                FROM public.locations
                WHERE (@City IS NULL OR city ILIKE '%' || @City || '%')
                  AND (@District IS NULL OR district ILIKE '%' || @District || '%')
                  AND (@PostalCode IS NULL OR postal_code ILIKE '%' || @PostalCode || '%')
                ORDER BY created_at DESC;";

            var conn = _db.DbConnection;
            return await conn.QueryAsync<LocationDTO>(sql, new { City = city, District = district, PostalCode = postalCode });
        }

        // Exists (Postgres-friendly)
        public async Task<bool> Exists(int locationId)
        {
            const string sql = @"SELECT EXISTS(SELECT 1 FROM public.locations WHERE location_id = @LocationId);";
            var conn = _db.DbConnection;
            return await conn.ExecuteScalarAsync<bool>(sql, new { LocationId = locationId });
        }
    }
}
