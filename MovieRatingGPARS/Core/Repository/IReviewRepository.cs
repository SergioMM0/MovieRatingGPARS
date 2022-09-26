using MovieRatingGPARS.Core.Model;

namespace MovieRatingGPARS.Core.Repository;

public interface IReviewRepository
{
    BEReview[] GetAll();
}