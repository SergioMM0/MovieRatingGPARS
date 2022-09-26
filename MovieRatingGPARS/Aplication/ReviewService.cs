using MovieRatingGPARS.Core.Model;
using MovieRatingGPARS.Core.Repository;
using MovieRatingGPARS.Core.Service;

namespace MovieRatingGPARS.Aplication;

public class ReviewService : IReviewService
{
    private IReviewRepository _repository;
    public ReviewService(IReviewRepository repository)
    {
        if (repository == null)
            throw new ArgumentException("Missing repository");
        _repository = repository;
    }

    public int GetNumberOfReviewsFromReviewer(int reviewer)
    {
        var count = 0;
        foreach (BEReview review in _repository.GetAll())
        {
            if (review.Reviewer == reviewer)
            {
                count++;
            }
        }
        return count;
    }
}