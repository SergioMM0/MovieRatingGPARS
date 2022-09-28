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
    
    //2nd method
    [Theory]
    [InlineData(1,5)]
    [InlineData(2,3)]
    [InlineData(3,0)]
    
    public void GetAverageRateFromReviewer(int reviewer, double expected)
    {
        //Arrange
        BEReview[] fakeRepo = new BEReview[]
        {
            //Reviewer 1 has an average of 5
            //Reviewer 2 has an average of 3
            new BEReview() { Reviewer = 1, Movie = 1, Grade = 10, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 2, Grade = 0, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 1, Grade = 4, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 2, Grade = 2, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 3, Grade = 10, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 2, Movie = 3, Grade = 3, ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = 4, Grade = 0, ReviewDate = new DateTime() }
        };
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepository.Object);

        //Act

        double result = service.GetAverageRateFromReviewer(reviewer);

        //Assert
        
        Assert.Equal(result, expected);
        mockRepository.Verify(r => r.GetAll(), Times.Once);
    }
    
    [Theory]
    [InlineData(new int[] { 1, 3, 3, 2, 2, 4, 8 },new int []{2,3})]
    [InlineData(new int[] { 1, 2, 3, 1, 1, 1, 2 },new int []{2})]
    [InlineData(new int[] { 1, 2, 3, 1, 2, 3, 0 },new int []{1,2,3})]

    
    public void GetMostProductiveReviewers(int [] reviewrsIds,int[] mostActiveReviewers)
    {
        //Arrange
        var fakeRepo = new BEReview[reviewrsIds.Length];
        
        for (int i = 0; i < reviewrsIds.Length; i++)
        {
            fakeRepo[i].Reviewer = reviewrsIds[i];
        }
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepository.Object);


        //Act

        int[] result = service.GetMostProductiveReviewers().ToArray();

        //Assert
        
        Assert.Equal(result,mostActiveReviewers);
        Assert.True(result.Length==mostActiveReviewers.Length);
        
        
        mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

}