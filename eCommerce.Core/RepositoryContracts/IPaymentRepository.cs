using eCommerce.Core.DTO.NewFolder;
using eCommerce.Core.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IPaymentRepository
    {
        Task<PaymentDTO?> GetById(Guid paymentId);
        Task<IEnumerable<PaymentDTO>> GetByBooking(Guid bookingId);

        Task<PaymentDTO> Create(PaymentCreateDTO dto);
        Task<PaymentDTO?> Update(Guid paymentId, PaymentUpdateDTO dto);
        Task<bool> Delete(Guid paymentId);

        Task<bool> MarkPaid(Guid paymentId, DateTime? paidAt, string? transactionId);
        Task<bool> Exists(Guid paymentId);
    }
    
}
