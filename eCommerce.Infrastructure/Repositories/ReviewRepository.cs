using Dapper;
using eCommerce.Core.DTO.Review;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class ReviewRepository :IReviewRepository
    {

            private readonly DapperDbContext _db;
            public ReviewRepository(DapperDbContext db) => _db = db;

            // Read one
            public async Task<ReviewDTO?> GetById(int reviewId)
            {
                const string sql = @"
                SELECT
                    review_id   AS ""ReviewId"",
                    booking_id  AS ""BookingId"",
                    customer_id AS ""CustomerId"",
                    cleaner_id  AS ""CleanerId"",
                    rating      AS ""Rating"",
                    comment     AS ""Comment"",
                    created_at  AS ""CreatedAt""
                FROM public.reviews
                WHERE review_id = @Id;";
                return await _db.DbConnection.QueryFirstOrDefaultAsync<ReviewDTO>(sql, new { Id = reviewId });
            }

            // One-per-booking rule support
            public async Task<ReviewDTO?> GetByBooking(Guid bookingId)
            {
                const string sql = @"
                SELECT
                    review_id   AS ""ReviewId"",
                    booking_id  AS ""BookingId"",
                    customer_id AS ""CustomerId"",
                    cleaner_id  AS ""CleanerId"",
                    rating      AS ""Rating"",
                    comment     AS ""Comment"",
                    created_at  AS ""CreatedAt""
                FROM public.reviews
                WHERE booking_id = @BookingId
                LIMIT 1;";
                return await _db.DbConnection.QueryFirstOrDefaultAsync<ReviewDTO>(sql, new { BookingId = bookingId });
            }

            // List by cleaner
            public async Task<IEnumerable<ReviewDTO>> GetByCleaner(Guid cleanerId, int? limit = null, int? offset = null)
            {
                var sql = @"
                SELECT
                    review_id   AS ""ReviewId"",
                    booking_id  AS ""BookingId"",
                    customer_id AS ""CustomerId"",
                    cleaner_id  AS ""CleanerId"",
                    rating      AS ""Rating"",
                    comment     AS ""Comment"",
                    created_at  AS ""CreatedAt""
                FROM public.reviews
                WHERE cleaner_id = @CleanerId
                ORDER BY created_at DESC";
                if (limit.HasValue) sql += " LIMIT @Limit";
                if (offset.HasValue) sql += " OFFSET @Offset";

                return await _db.DbConnection.QueryAsync<ReviewDTO>(sql, new { CleanerId = cleanerId, Limit = limit, Offset = offset });
            }

            // List by customer
            public async Task<IEnumerable<ReviewDTO>> GetByCustomer(Guid customerId, int? limit = null, int? offset = null)
            {
                var sql = @"
                SELECT
                    review_id   AS ""ReviewId"",
                    booking_id  AS ""BookingId"",
                    customer_id AS ""CustomerId"",
                    cleaner_id  AS ""CleanerId"",
                    rating      AS ""Rating"",
                    comment     AS ""Comment"",
                    created_at  AS ""CreatedAt""
                FROM public.reviews
                WHERE customer_id = @CustomerId
                ORDER BY created_at DESC";
                if (limit.HasValue) sql += " LIMIT @Limit";
                if (offset.HasValue) sql += " OFFSET @Offset";

                return await _db.DbConnection.QueryAsync<ReviewDTO>(sql, new { CustomerId = customerId, Limit = limit, Offset = offset });
            }

            // Create
            public async Task<ReviewDTO> Create(ReviewCreateDTO dto)
            {
                const string sql = @"
                INSERT INTO public.reviews
                    (booking_id, customer_id, cleaner_id, rating, comment, created_at)
                VALUES
                    (@BookingId, @CustomerId, @CleanerId, @Rating, @Comment, NOW())
                RETURNING
                    review_id   AS ""ReviewId"",
                    booking_id  AS ""BookingId"",
                    customer_id AS ""CustomerId"",
                    cleaner_id  AS ""CleanerId"",
                    rating      AS ""Rating"",
                    comment     AS ""Comment"",
                    created_at  AS ""CreatedAt"";";

                return await _db.DbConnection.QuerySingleAsync<ReviewDTO>(sql, dto);
            }

            // Update (partial — respects nullable fields in ReviewUpdateDTO)
            public async Task<ReviewDTO?> Update(int reviewId, ReviewUpdateDTO dto)
            {
                const string sql = @"
                UPDATE public.reviews
                SET
                    rating  = COALESCE(@Rating, rating),
                    comment = COALESCE(@Comment, comment)
                WHERE review_id = @Id
                RETURNING
                    review_id   AS ""ReviewId"",
                    booking_id  AS ""BookingId"",
                    customer_id AS ""CustomerId"",
                    cleaner_id  AS ""CleanerId"",
                    rating      AS ""Rating"",
                    comment     AS ""Comment"",
                    created_at  AS ""CreatedAt"";";

                return await _db.DbConnection.QueryFirstOrDefaultAsync<ReviewDTO>(sql,
                    new { Id = reviewId, dto.Rating, dto.Comment });
            }

            public async Task<bool> Delete(int reviewId)
            {
                const string sql = @"DELETE FROM public.reviews WHERE review_id = @Id;";
                var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = reviewId });
                return rows > 0;
            }

            public async Task<bool> Exists(int reviewId)
            {
                const string sql = @"SELECT EXISTS(SELECT 1 FROM public.reviews WHERE review_id = @Id);";
                return await _db.DbConnection.ExecuteScalarAsync<bool>(sql, new { Id = reviewId });
            }

            public async Task<(double Average, int Count)> GetCleanerRatingSummary(Guid cleanerId)
            {
                const string sql = @"
                SELECT
                    COALESCE(AVG(rating), 0)::float8 AS Average,
                    COUNT(*)                         AS Count
                FROM public.reviews
                WHERE cleaner_id = @CleanerId;";

                var row = await _db.DbConnection.QuerySingleAsync<(double Average, int Count)>(sql, new { CleanerId = cleanerId });
                return (row.Average, row.Count);
            }
        }
    
}
