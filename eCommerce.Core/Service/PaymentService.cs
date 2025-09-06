using eCommerce.Core.DTO.NewFolder;
using eCommerce.Core.DTO.Payment;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;

        public PaymentService(IPaymentRepository repo) => _repo = repo;

        public Task<PaymentDTO?> GetByIdAsync(Guid paymentId) => _repo.GetById(paymentId);
        public Task<IEnumerable<PaymentDTO>> GetByBookingAsync(Guid bookingId) => _repo.GetByBooking(bookingId);

        public async Task<PaymentDTO> CreateAsync(PaymentCreateDTO dto)
        {
            if (dto.Amount <= 0) throw new ArgumentOutOfRangeException(nameof(dto.Amount), "Amount must be > 0.");
            if (string.IsNullOrWhiteSpace(dto.Method)) throw new ArgumentException("Method is required.", nameof(dto.Method));
            return await _repo.Create(dto);
        }

        public async Task<PaymentDTO?> UpdateAsync(Guid paymentId, PaymentUpdateDTO dto)
        {
            if (dto.PaymentId == Guid.Empty) dto.PaymentId = paymentId;
            else if (dto.PaymentId != paymentId) throw new ArgumentException("Route id and body PaymentId mismatch.");
            if (dto.Amount.HasValue && dto.Amount <= 0) throw new ArgumentOutOfRangeException(nameof(dto.Amount), "Amount must be > 0.");
            return await _repo.Update(paymentId, dto);
        }

        public Task<bool> DeleteAsync(Guid paymentId) => _repo.Delete(paymentId);

        public Task<bool> MarkPaidAsync(Guid paymentId, DateTime? paidAt = null, string? transactionId = null)
            => _repo.MarkPaid(paymentId, paidAt, transactionId);
    }
}

