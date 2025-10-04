using eCommerce.Core.DTO.Booking;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingDTO>> ListForCleanerAsync(
         
            string status,
            DateTime? from,
            DateTime? to,
            Guid? Cleaid
        );
        Task<int> CountBooking();
        Task<BookingDTO?> GetById(Guid bookingId);
        Task<IEnumerable<BookingDTO>> GetByCustomer(Guid customerId, DateTime? from, DateTime? to);
        Task<IEnumerable<BookingDTO>> GetByCleaner(Guid cleanerId, DateTime? from, DateTime? to);

        Task<BookingDTO> Create(BookingCreateDTO dto);
        Task<BookingDTO?> Update(Guid bookingId, BookingUpdateDTO dto);
        Task<bool> Delete(Guid bookingId);

        Task<bool> ChangeStatus(Guid bookingId, string status, Guid cleanerId);

        Task<bool> ExistsAsync(Guid bookingId);
        Task<IReadOnlyList<BookingDTO>> GetMyBooking(Guid? customerId);

    }
}
