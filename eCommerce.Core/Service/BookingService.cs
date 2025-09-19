using eCommerce.Core.DTO.Booking;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;

        public BookingService(IBookingRepository repo) => _repo = repo;

        public Task<BookingDTO?> GetByIdAsync(Guid bookingId) => _repo.GetById(bookingId);
        public Task<IEnumerable<BookingDTO>> GetByCustomerAsync(Guid customerId, DateTime? from, DateTime? to)
            => _repo.GetByCustomer(customerId, from, to);
        public Task<IEnumerable<BookingDTO>> GetByCleanerAsync(Guid cleanerId, DateTime? from, DateTime? to)
            => _repo.GetByCleaner(cleanerId, from, to);

        public async Task<BookingDTO> CreateAsync(BookingCreateDTO dto)
        {
            // basic validation
            if (dto.BookingDate == default) throw new ArgumentException("BookingDate is required.");
            return await _repo.Create(dto);
        }

        public async Task<BookingDTO?> UpdateAsync(Guid bookingId, BookingUpdateDTO dto)
        {
            if (dto.BookingId == Guid.Empty) dto.BookingId = bookingId;
            else if (dto.BookingId != bookingId) throw new ArgumentException("Route id and body BookingId mismatch.");
            return await _repo.Update(bookingId, dto);
        }

        public Task<bool> DeleteAsync(Guid bookingId) => _repo.Delete(bookingId);

        public async Task<bool> ConfirmAsync(Guid bookingId, Guid cleanerId)
        {

            var affected = await _repo.ChangeStatus(bookingId, "Confirmed", cleanerId);

            // affected = 1 ⇒ success, else fail
            return affected;
        }
        public async Task<bool> CompleteAsync(Guid bookingId, Guid cleanerId)
        {
            var affected = await _repo.ChangeStatus(bookingId, "Completed", cleanerId);

            // affected = 1 ⇒ success, else fail
            return affected;
        }


      
            
      
        public async Task<bool> CancelAsync(Guid bookingId, Guid cleanerId)

        {

            var affected = await _repo.ChangeStatus(bookingId, "Confirmed", cleanerId);

            // affected = 1 ⇒ success, else fail
            return affected;
        }
        public async Task<IEnumerable<BookingDTO>> ListForCleanerAsync(string status, DateTime? from, DateTime? to)
        {
            var rows = await _repo.ListForCleanerAsync(status, from, to);
            return (IEnumerable<BookingDTO>)rows;
        }

        public async Task<int> CountBooking()
        {
            return await _repo.CountBooking();
        }
    }
}
