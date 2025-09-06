using eCommerce.Core.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface IReviewService
    {
        Task<ReviewDTO?> GetByIdAsync(int reviewId);
        Task<ReviewDTO?> GetByBookingAsync(Guid bookingId); // if one review per booking
        Task<IEnumerable<ReviewDTO>> GetByCleanerAsync(Guid cleanerId, int? limit = null, int? offset = null);
        Task<IEnumerable<ReviewDTO>> GetByCustomerAsync(Guid customerId, int? limit = null, int? offset = null);

        // Create / Update / Delete
        Task<ReviewDTO> CreateAsync(ReviewCreateDTO dto);
        Task<ReviewDTO?> UpdateAsync(int reviewId, ReviewUpdateDTO dto);
        Task<bool> DeleteAsync(int reviewId);

        // Helpers
        Task<bool> ExistsAsync(int reviewId);
        Task<(double Average, int Count)> GetCleanerRatingSummaryAsync(Guid cleanerId);

    }
}
