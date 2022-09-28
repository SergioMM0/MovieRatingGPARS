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

    /*
     * Working currently but not sure what will happen in the case of movies with the same ratings
     */
    public List<int> GetTopRatedMovies(int amount)
    {
        //Create a sorted list with the double(rating) being the key
        //and the movie id being the value in the kvp.
        SortedList<double, int> ratingList = new SortedList<double, int>();
        IEnumerable<BEReview> reviews = _repository.GetAll().ToList();

        //We get all the unique id's of the movies, as to not calculate the average for the same movie twice
        List<int> uniqueMovieIds = reviews.Select(r => r.Movie).Distinct().ToList();
        for (int i = 0; i< uniqueMovieIds.Count(); i++)
        {
            //Create variables for each movie to keep track and be able to calculate average rating later
            int ratingSum = 0;
            int count = 0;
            double averageRating = 0;

            //Get all occurrences of ratings for a specific movie
            IEnumerable<BEReview> occurences = reviews.Where(review => review.Movie==uniqueMovieIds[i]);
            
            //Increment values for each rating on current movie
            foreach (var review in occurences)
            {
                count++;
                ratingSum += review.Grade;
            }

            //Check if any ratings actually exist, otherwise a divide by zero will occur;
            if (count!=0)
                averageRating = (double)ratingSum / count;
            ratingList.Add(averageRating,uniqueMovieIds[i]);
        }

        List<double> keysInOrder = ratingList.Keys.OrderByDescending(k=>k).ToList();
        List<int> ratingsInOrder = retrieveValuesByKeys(keysInOrder,ratingList);

        return ratingsInOrder.Take(amount).ToList();
    }

    private List<int> retrieveValuesByKeys(List<double> keysInOrder, SortedList<double, int> source)
    {
        List<int> valuesInOrder = new List<int>();
        foreach (double currentKey in keysInOrder)
        {
            int value = 0;
            source.TryGetValue(currentKey, out value);
            valuesInOrder.Add(value);
        }

        return valuesInOrder;

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