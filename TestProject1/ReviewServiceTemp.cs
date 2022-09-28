using Moq;
using MovieRatingGPARS.Aplication;
using MovieRatingGPARS.Core.Model;
using MovieRatingGPARS.Core.Repository;

namespace TestProject1;

public class ReviewServiceTemp
{
    
    //3
    [Theory]
    [InlineData(1,1,1)]
    [InlineData(1,3,3)]
    [InlineData(1,4,2)]
    [InlineData(1,5,0)]
    public void GetNumberOfRatesByReviewer(int reviewer,int rating,int expectedCount)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 1, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 4, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 4, ReviewDate = new DateTime() }
        };
        
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        ReviewService service = new ReviewService(mockRepository.Object);
        //Act
        int actualCount = service.GetNumberOfRatesByReviewer(reviewer, rating);
        //Assert
        Assert.Equal(expectedCount,actualCount);
        
    }

    //9
    [Theory]
    [InlineData(1,4)]
    [InlineData(2,4,2)]
    [InlineData(3,4,2,6)]
    [InlineData(0)]
    public void GetTopRatedMovies(int countOfMoviesRequested,params int[] expectedTopMovies)
    {
        //Arrange
        int movieCount = 4;
        
        #region coolRegionOfData

        BEReview[] data = new BEReview[]
        {
            new BEReview(1,2,5,new DateTime()),
            new BEReview(1,2,4,new DateTime()),
            new BEReview(1,2,4,new DateTime()),
            new BEReview(1,2,3,new DateTime()),
            
            new BEReview(1,3,1,new DateTime()),
            new BEReview(1,3,2,new DateTime()),
            new BEReview(1,3,1,new DateTime()),
            new BEReview(1,3,0,new DateTime()),
            new BEReview(1,3,0,new DateTime()),
            
            new BEReview(1,1,2,new DateTime()),
            new BEReview(1,1,3,new DateTime()),
            new BEReview(1,1,3,new DateTime()),
            new BEReview(1,1,4,new DateTime()),
            
            new BEReview(1,4,5,new DateTime()),
            new BEReview(1,4,5,new DateTime()),
            new BEReview(1,4,5,new DateTime()),
            new BEReview(1,4,5,new DateTime()),
            
            new BEReview(1,6,5,new DateTime()),
            new BEReview(1,6,4,new DateTime()),
            new BEReview(1,6,4,new DateTime()),
            new BEReview(1,6,3,new DateTime()),
        };
        #endregion


        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(data);
        
        ReviewService service = new ReviewService(mockRepository.Object);
        
        //Act
        int[] coolData = service.GetTopRatedMovies(countOfMoviesRequested).ToArray();
        
        //Assert
        Assert.Equal(expectedTopMovies,coolData);
    }

    //6
    [Theory]
    [InlineData(1,3,2)]
    [InlineData(2,0,0)]
    [InlineData(3,2,1)]
    [InlineData(4,5,4)]
    [InlineData(5,0,0)]
    public void GetNumberOfRatesTest(int movie,int rating,int expectedOccurrenceCount)
    {
        //Arrange
        #region coolRegionOfData

        BEReview[] data = new BEReview[]
        {
            new BEReview(1,2,5,new DateTime()),
            new BEReview(1,2,4,new DateTime()),
            new BEReview(1,2,4,new DateTime()),
            new BEReview(1,2,3,new DateTime()),
            
            new BEReview(1,3,1,new DateTime()),
            new BEReview(1,3,2,new DateTime()),
            new BEReview(1,3,1,new DateTime()),
            new BEReview(1,3,0,new DateTime()),
            new BEReview(1,3,0,new DateTime()),
            
            new BEReview(1,1,2,new DateTime()),
            new BEReview(1,1,3,new DateTime()),
            new BEReview(1,1,3,new DateTime()),
            new BEReview(1,1,4,new DateTime()),
            
            new BEReview(1,4,5,new DateTime()),
            new BEReview(1,4,5,new DateTime()),
            new BEReview(1,4,5,new DateTime()),
            new BEReview(1,4,5,new DateTime()),
        };
        #endregion

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).Returns(data);
        ReviewService service = new ReviewService(mockRepo.Object);
        
        //Act
        int actualOccurrenceCount = service.GetNumberOfRates(movie,rating);
        //Assert
        Assert.Equal(expectedOccurrenceCount,actualOccurrenceCount);
    }

    //11
    [Theory]
    [InlineData(2,1,2,3,4)]
    [InlineData(3,1)]
    [InlineData(1,1,3,4)]
    [InlineData(4,1,2)]
    [InlineData(5)]
    public void GetReviewersByMovieTest(int movie,params int[] expectedUsers)
    {
        //Arrange
        #region coolRegionOfData

        BEReview[] data = new BEReview[]
        {
            new BEReview(1,2,5,new DateTime()),
            new BEReview(2,2,4,new DateTime()),
            new BEReview(3,2,4,new DateTime()),
            new BEReview(4,2,3,new DateTime()),
            
            new BEReview(1,3,1,new DateTime()),
            
            new BEReview(1,1,2,new DateTime()),
            new BEReview(3,1,3,new DateTime()),
            new BEReview(4,1,3,new DateTime()),

            new BEReview(1,4,5,new DateTime()),
            new BEReview(2,4,5,new DateTime())
        };
        #endregion

        Mock<IReviewRepository> mockRepo = new Mock<IReviewRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).Returns(data);

        ReviewService service = new ReviewService(mockRepo.Object);
        
        //Act
        int[] actualUsers = service.GetReviewersByMovie(movie).ToArray();
        //Assert
        Assert.Equal(expectedUsers,actualUsers);
    }
}