using eCommerce.Core.DTO.NewFolder;
using eCommerce.Core.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface IPaymentService
    {
        Task<PaymentDTO?> GetByIdAsync(Guid paymentId);
        Task<IEnumerable<PaymentDTO>> GetByBookingAsync(Guid bookingId);

        Task<PaymentDTO> CreateAsync(PaymentCreateDTO dto);
        Task<PaymentDTO?> UpdateAsync(Guid paymentId, PaymentUpdateDTO dto);
        Task<bool> DeleteAsync(Guid paymentId);

        Task<bool> MarkPaidAsync(Guid paymentId, DateTime? paidAt = null, string? transactionId = null);

    }
}
