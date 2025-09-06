using eCommerce.Core.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface IBookingService
    {
        Task<BookingDTO?> GetByIdAsync(Guid bookingId);
        Task<IEnumerable<BookingDTO>> GetByCustomerAsync(Guid customerId, DateOnly? from, DateOnly? to);
        Task<IEnumerable<BookingDTO>> GetByCleanerAsync(Guid cleanerId, DateOnly? from, DateOnly? to);

        Task<BookingDTO> CreateAsync(BookingCreateDTO dto);
        Task<BookingDTO?> UpdateAsync(Guid bookingId, BookingUpdateDTO dto);
        Task<bool> DeleteAsync(Guid bookingId);

        // Status transitions
        Task<bool> ConfirmAsync(Guid bookingId);
        Task<bool> CompleteAsync(Guid bookingId);
        Task<bool> CancelAsync(Guid bookingId);
    }
}
