using eCommerce.Core.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IReviewRepository
    {
        Task<ReviewDTO?> GetById(int reviewId);
        Task<ReviewDTO?> GetByBooking(Guid bookingId);                        // one review per booking (if your rule)
        Task<IEnumerable<ReviewDTO>> GetByCleaner(Guid cleanerId, int? limit = null, int? offset = null);
        Task<IEnumerable<ReviewDTO>> GetByCustomer(Guid customerId, int? limit = null, int? offset = null);

        // Create / Update / Delete
        Task<ReviewDTO> Create(ReviewCreateDTO dto);
        Task<ReviewDTO?> Update(int reviewId, ReviewUpdateDTO dto);
        Task<bool> Delete(int reviewId);

        // Helpers
        Task<bool> Exists(int reviewId);
        Task<(double Average, int Count)> GetCleanerRatingSummary(Guid cleanerId); // avg + count for stars

    }
}
