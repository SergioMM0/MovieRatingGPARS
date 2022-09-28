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
}