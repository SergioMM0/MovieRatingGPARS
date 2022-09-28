using Moq;
using MovieRatingGPARS.Aplication;
using MovieRatingGPARS.Core.Model;
using MovieRatingGPARS.Core.Repository;

namespace TestProject1;

public class ReviewServiceTemp
{
    
    [Theory]
    [InlineData(1,1,1)]
    [InlineData(1,3,3)]
    [InlineData(1,4,2)]
    [InlineData(1,5,0)]
    public void GetAverageRateFromReviewer(int reviewer,int rating,int expectedCount)
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

    [Fact]
    public void GetTopRatedMovies()
    {
        //Arrange
        int movieCount = 3;
        
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

        List<int> expectedMovieIds = new List<int>() {4,2,1};

        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(data);
        
        ReviewService service = new ReviewService(mockRepository.Object);
        //Act

        List<int> coolData = service.GetTopRatedMovies(movieCount);
        //Assert
        Assert.Equal(expectedMovieIds,coolData);
    }

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
}