namespace MovieRatingGPARS.Core.Service;

public interface IReviewService
{
    int GetNumberOfReviewsFromReviewer(int reviewer);
}