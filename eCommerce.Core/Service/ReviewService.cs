using eCommerce.Core.DTO.Review;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public Task<ReviewDTO?> GetByIdAsync(int reviewId) => _repo.GetById(reviewId);

        public Task<ReviewDTO?> GetByBookingAsync(Guid bookingId) => _repo.GetByBooking(bookingId);

        public Task<IEnumerable<ReviewDTO>> GetByCleanerAsync(Guid cleanerId, int? limit = null, int? offset = null)
            => _repo.GetByCleaner(cleanerId, limit, offset);

        public Task<IEnumerable<ReviewDTO>> GetByCustomerAsync(Guid customerId, int? limit = null, int? offset = null)
            => _repo.GetByCustomer(customerId, limit, offset);

  
        public async Task<ReviewDTO> CreateAsync(ReviewCreateDTO dto)
        {
            ValidateRating(dto.Rating);

            var existing = await _repo.GetByBooking(dto.BookingId);
            if (existing is not null)
                throw new InvalidOperationException("This booking already has a review.");

            return await _repo.Create(dto);
        }


        public async Task<ReviewDTO?> UpdateAsync(int reviewId, ReviewUpdateDTO dto)
        {
            if (dto.ReviewId == 0) dto.ReviewId = reviewId;
            else if (dto.ReviewId != reviewId)
                throw new ArgumentException("Route id and body ReviewId mismatch.");

            if (dto.Rating.HasValue) ValidateRating(dto.Rating.Value);

            return await _repo.Update(reviewId, dto);
        }


        public Task<bool> DeleteAsync(int reviewId) => _repo.Delete(reviewId);

        public Task<bool> ExistsAsync(int reviewId) => _repo.Exists(reviewId);

        public Task<(double Average, int Count)> GetCleanerRatingSummaryAsync(Guid cleanerId)
            => _repo.GetCleanerRatingSummary(cleanerId);

        private static void ValidateRating(int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
        }
    }
}
