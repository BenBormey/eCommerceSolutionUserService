using eCommerce.Core.DTO.Booking;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
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
        Task<IEnumerable<BookingDTO>> GetByCustomerAsync(Guid customerId, DateTime? from, DateTime? to);
        Task<IEnumerable<BookingDTO>> GetByCleanerAsync(Guid cleanerId, DateTime? from, DateTime? to);

        Task<BookingDTO> CreateAsync(BookingCreateDTO dto);
        Task<BookingDTO?> UpdateAsync(Guid bookingId, BookingUpdateDTO dto);
        Task<bool> DeleteAsync(Guid bookingId);

        // Status transitions
        Task<bool> ConfirmAsync(Guid bookingId, Guid cleanerId);
        Task<bool> CompleteAsync(Guid bookingId, Guid cleanerId);
        Task<bool> CancelAsync(Guid bookingId, Guid cleanerId);
        Task<IEnumerable<BookingDTO>> ListForCleanerAsync( string status, DateTime? from, DateTime? to, Guid? cleaid);
        Task<int> CountBooking();
        Task<IReadOnlyList<BookingDTO>> GetMyBooking(Guid? customerId);
    }
}
