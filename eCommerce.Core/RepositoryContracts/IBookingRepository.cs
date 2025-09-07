using eCommerce.Core.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IBookingRepository
    {
        Task<BookingDTO?> GetById(Guid bookingId);
        Task<IEnumerable<BookingDTO>> GetByCustomer(Guid customerId, DateTime? from, DateTime? to);
        Task<IEnumerable<BookingDTO>> GetByCleaner(Guid cleanerId, DateTime? from, DateTime? to);

        Task<BookingDTO> Create(BookingCreateDTO dto);
        Task<BookingDTO?> Update(Guid bookingId, BookingUpdateDTO dto);
        Task<bool> Delete(Guid bookingId);

        Task<bool> ChangeStatus(Guid bookingId, string status);
        Task<bool> ExistsAsync(Guid bookingId);
    }
}
