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
        return _repository.GetAll().Count(review => review.Reviewer == reviewer);
    }

    public int GetNumberOfRatesByReviewer(int reviewer, int rate)
    {
        return _repository.GetAll().Count(review => reviewer == review.Reviewer && rate == review.Grade);
    }
    public  double GetAverageRateFromReviewer(int reviewer)
    {
        IEnumerable<BEReview> reviews = _repository.GetAll().Where(review => review.Reviewer == reviewer);
        return reviews.Sum(review => review.Grade) / reviews.Count();
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
        return reviews.Count();
    }

    //method implemented but still needs to be tested.
    public List<int> GetMoviesWithHighestNumberOfTopRates()
    {
        return _repository.GetAll()
            .Where(review => review.Grade == 5)
            .Select(review => review.Movie)
            .ToList();
    }

    public List<int> GetMostProductiveReviewers()
    {
        var dictionary = new Dictionary<int,int>();
        for (var i = 0; i < _repository.GetAll().Length; i++)
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

        var maxValueKey = dictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Value;

        return (from item in dictionary where item.Value >= maxValueKey select item.Key).ToList();
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
    //method implemented but still needs to be tested.
    public List<int> GetTopMoviesByReviewer(int reviewer)
    {
        IEnumerable reviews = _repository.GetAll().Where(review => review.Reviewer == reviewer);
        return reviews.Cast<BEReview>().ToList().OrderBy(review => review.Grade).ThenBy(review =>review.ReviewDate).Select(review => review.Reviewer).ToList();
    }

    public List<int> GetReviewersByMovie(int movie)
    {
        return _repository.GetAll().Where(r => r.Movie == movie).Select(r => r.Reviewer).ToList();
    }
}