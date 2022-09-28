using System.Collections;
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
        IEnumerable<BEReview> reviews = _repository.GetAll()
            .Where(r=>r.Movie==movie)
            .Where(r=>r.Grade==rate);
        int occurences = reviews.Count();
        return occurences;
    }

    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        throw new NotImplementedException();
    }

    public List<int> GetMostProductiveReviewers()
    {
        var dictionary = new Dictionary<int,int>();
        for (int i = 0; i < _repository.GetAll().Length; i++)
        { 
            if (!dictionary.ContainsKey(_repository.GetAll()[i].Reviewer))
                dictionary.Add(_repository.GetAll()[i].Reviewer,1);
            
            for (int j = i+1; j < _repository.GetAll().Length-1; j++)
            {
                if (_repository.GetAll()[j].Reviewer==_repository.GetAll()[i].Reviewer)
                {
                    dictionary[_repository.GetAll()[i].Reviewer] = int.Parse(dictionary[_repository.GetAll()[i].Reviewer].ToString())+1;
                }
            }
        }

        List<int> mostActiveReviewers=new List<int>();
        var maxValueKey = dictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Value;
        foreach (var item in dictionary)
        {
            if (item.Value>=maxValueKey)
            {
                mostActiveReviewers.Add(item.Key);
            }
        }

        return mostActiveReviewers;
    }

    
    /*
     * Method changed to allow duplicate values and if such an occurrence happens it will then look for the 2nd value
     * to sort by which in this case we choose the id, so it will sort by the review grade descending and after that by
     * the id in ascending order
     */
    public List<int> GetTopRatedMovies(int amount)
    {
        //Create a list of KVP to map the key(being the average rating, a double) with the value (movie id)
        List<KeyValuePair<double, int>> ratingList = new List<KeyValuePair<double, int>>();
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
            ratingList.Add(new KeyValuePair<double, int>(averageRating,uniqueMovieIds[i]));
        }

        //Order the list 1st by the key (rating) in a descending order, and just in case of exact same average ratings
        //We also sort by id's in an ascending order
        ratingList = ratingList.OrderByDescending(pair => pair.Key).ThenBy(pair => pair.Value).ToList();
        //Select only the values from the kvp list
        List<int> moviesInOrder = ratingList.Select(pair => pair.Value).ToList();

        //Take the top N movies of the list
        return moviesInOrder.Take(amount).ToList();
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
        IEnumerable<BEReview> reviewsForMovie = _repository.GetAll().Where(r => r.Movie == movie);
        List<int> reviewersForMovie = reviewsForMovie.Select(r => r.Reviewer).ToList();
        return reviewersForMovie;
    }
}