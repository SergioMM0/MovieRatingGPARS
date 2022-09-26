using Moq;
using MovieRatingGPARS.Aplication;
using MovieRatingGPARS.Core.Model;
using MovieRatingGPARS.Core.Repository;
using MovieRatingGPARS.Core.Service;

namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void CreateReviewService()
    {
        //Arrange
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        IReviewRepository repository = mockRepository.Object; 
        //creates a functional 

        //Act
        IReviewService service = new ReviewService(repository);
        
        //Assert
        Assert.NotNull(service);
        Assert.True(service is ReviewService);
    }

    [Fact]
    public void CreateReviewServiceWithNullRepositoryExceptArgumentException()
    {
        //Arrange
        IReviewService service = null;
        
        //Act + Assert
        var ex = Assert.Throws<ArgumentException>(() =>service = new ReviewService(null));
        Assert.Equal("Missing repository", ex.Message);
        Assert.Null(service);
    }

    [Theory]
    [InlineData(1,2)]
    [InlineData(2,1)]
    [InlineData(3,0)]
    public void GetNumberOfReviewsFromReviewer(int reviewer, int expected)
    {
        BEReview[] fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 3, ReviewDate = new DateTime() }
        };

        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);

        IReviewService service = new ReviewService(mockRepository.Object);
        
        //Act
        int result = service.GetNumberOfReviewsFromReviewer(reviewer);
        
        //Assert
        Assert.Equal(expected, result);
        mockRepository.Verify(r => r.GetAll(), Times.Once);
    }
}