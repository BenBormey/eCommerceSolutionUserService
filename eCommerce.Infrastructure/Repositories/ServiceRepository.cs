using Dapper;
using eCommerce.Core.DTO.Service;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DapperDbContext _context;
        public ServiceRepository(DapperDbContext context)
        {
            this._context = context;

        }
        public async Task<ServiceDTO> Create(ServiceCreateDTO cre)
        {
            var query = @"
        INSERT INTO public.services 
            (name, description, price, duration_minutes, image_url, is_active, created_at)
        VALUES 
            (@Name, @Description, @Price, @DurationMinutes, @ImageUrl, @IsActive, NOW())
        RETURNING 
            service_id, name, description, price, duration_minutes, image_url, is_active, created_at;
    ";

            var result = await _context.DbConnection.QuerySingleAsync<ServiceDTO>(query, cre);
            return result;
        }

        public async  Task<bool> Delete(int serviceId)
        {
            var query = @"DELETE FROM public.services WHERE service_id = @ServiceId;";

            var affectedRows = await _context.DbConnection.ExecuteAsync(query, new { ServiceId = serviceId });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<ServiceDTO>> GetService()
        {
            var query = @"SELECT 
    s.service_id as ServiceId ,
    s.name,
    s.description,
    s.price,
    s.duration_minutes as DurationMinutes,
    s.image_url as ImageUrl,
    s.is_active as IsActive,
    s.created_at
FROM public.services AS s
ORDER BY s.created_at DESC;
";
            var result  = await _context.DbConnection.QueryAsync<ServiceDTO>(query);
            return result;
        }

        public async Task<ServiceDTO?> GetServiceById(int serviceId)
        {
            var query = @"
        SELECT 
            s.service_id,
            s.name,
            s.description,
            s.price,
            s.duration_minutes,
            s.image_url,
            s.is_active,
            s.created_at
        FROM public.services AS s
        WHERE s.service_id = @ServiceId;
    ";

            var result = await _context.DbConnection.QueryFirstOrDefaultAsync<ServiceDTO>(
                query, new { ServiceId = serviceId });

            return result;
        }

        public async Task<bool> ToggleActiveAsync(int serviceId, bool isActive)
        {
            const string sql = @"
        UPDATE public.services
        SET is_active = @isActive
        WHERE service_id = @serviceId;";
            var rows = await _context.DbConnection.ExecuteAsync(sql, new { ServiceId = serviceId, IsActive = isActive });
            return rows > 0;
        }

        public async Task<ServiceDTO?> Update(int serviceId, ServiceUpdateDTO updateDto)
        {
            var query = @"
        UPDATE public.services
        SET 
            name = @Name,
            description = @Description,
            price = @Price,
            duration_minutes = @DurationMinutes,
            image_url = @ImageUrl,
            is_active = @IsActive
        WHERE service_id = @ServiceId
        RETURNING 
            service_id, name, description, price, duration_minutes, image_url, is_active, created_at;
    ";

            var result = await _context.DbConnection.QueryFirstOrDefaultAsync<ServiceDTO>(
                query,
                new
                {
                    ServiceId = serviceId,
                    updateDto.Name,
                    updateDto.Description,
                    updateDto.Price,
                    updateDto.DurationMinutes,
                    updateDto.ImageUrl,
                    updateDto.IsActive
                });

            return result;
        }
    }
}
