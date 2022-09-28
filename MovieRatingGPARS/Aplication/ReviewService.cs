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

    public int GetNumberOfRatesByReviewer(int reviewer, int rate)
    {
        int count = 0;

        foreach (BEReview review in _repository.GetAll())
        {
            if (reviewer == review.Reviewer && rate == review.Grade)
            {
                count++;
            }
        }

        return count;
    }
    public  double GetAverageRateFromReviewer(int reviewer)
    {
        var count = 0;
        var total = 0;
        foreach (BEReview review in _repository.GetAll())
        {
            if (review.Reviewer == reviewer)
            {
                count++;
                total += review.Grade;
            }
        }

        if (count!=0)
            return total/count;
        
            return 0;
        
    }
    

    public int GetNumberOfReviews(int movie)
    {
        throw new NotImplementedException();
    }

    public double GetAverageRateOfMovie(int movie)
    {
        throw new NotImplementedException();
    }

    public int GetNumberOfRates(int movie, int rate)
    {
        throw new NotImplementedException();
    }

    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        throw new NotImplementedException();
    }

    public List<int> GetMostProductiveReviewers()
    {
        throw new NotImplementedException();
    }

    public List<int> GetTopRatedMovies(int amount)
    {
        throw new NotImplementedException();
    }

    public List<int> GetTopMoviesByReviewer(int reviewer)
    {
        throw new NotImplementedException();
    }

    public List<int> GetReviewersByMovie(int movie)
    {
        throw new NotImplementedException();
    }
}