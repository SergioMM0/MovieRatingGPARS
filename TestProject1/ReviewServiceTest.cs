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

    
    //1
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
    
    //7
    [Fact]
    public void GetMoviesWithHighestNumberOfTopRatesThrowsInvalidOperationExWhenRepoIsEmpty()
    {
        //Arrange
        var fakeRepo = new BEReview[]{};

        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepository.Object);

        //Act & Assert
        
        Assert.Throws<InvalidOperationException>(() => service.GetMoviesWithHighestNumberOfTopRates());
    }
    
    //7
    [Theory]
    [InlineData(new int[]{1,2,3,4,5,6}, new int[]{2,5,5,4,0,5}, new int[]{2,3,6})]
    [InlineData(new int[]{1,2,3,4,5,6}, new int[]{2,3,4,5,0,1}, new int[]{4})]
    public void GetMoviesWithHighestNumberOfTopRates(int[] movieId, int[] grade, int[] expected)
    {
        //Arrange
        var fakeRepo = new BEReview[]
        {
            new BEReview() { Reviewer = 1, Movie = movieId[0], Grade = grade[0], ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = movieId[1], Grade = grade[1], ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = movieId[2], Grade = grade[2], ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = movieId[3], Grade = grade[3], ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = movieId[4], Grade = grade[4], ReviewDate = new DateTime() },
            new BEReview() { Reviewer = 1, Movie = movieId[5], Grade = grade[5], ReviewDate = new DateTime() }
        };
        
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepository.Object);
        
        //Act

        var actual = service.GetMoviesWithHighestNumberOfTopRates();

        //Assert
        
        Assert.Equal(expected,actual);
        mockRepository.Verify(r => r.GetAll(), Times.Once);
    }
    
    //8
    //Please do not change for the moment.
    //Debugging reveals correct behaviour but test keeps failing.
    [Theory]
    [InlineData(new int[] { 1, 3, 3, 2, 2, 4, 8 },new int []{2,3})]
    [InlineData(new int[] { 1, 2, 3, 1, 1, 1, 2 },new int []{1})]
    [InlineData(new int[] { 1, 2, 3, 1, 2, 3, 0 },new int []{1,2,3})]
    
    public void GetMostProductiveReviewers(int [] reviewrsIds,int[] mostActiveReviewers)
    {
        //Arrange
        var fakeRepo = new BEReview[reviewrsIds.Length];
        
        for (int i = 0; i < reviewrsIds.Length; i++)
        {
            fakeRepo[i] = new BEReview()
            {
                Reviewer = reviewrsIds[i]
            };

        }
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        IReviewService service = new ReviewService(mockRepository.Object);


        //Act
 
        var result = service.GetMostProductiveReviewers();

        //Assert
        Assert.True(result.All(mostActiveReviewers.Contains));
        Assert.True(result.Count==mostActiveReviewers.Length);
        
        
        mockRepository.Verify(r => r.GetAll(), Times.Once);
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
    
    //10 .

    [Fact]
    public void GetTopMoviesByReviewerThrowsInvalidOperationExceptionWhenReviewerHasNoReviews()
    {
        var reviewerWithNoReviews = 1; 
        
        //Initializes a fakeRepository
        var fakeRepo = new BEReview[]
        {
            new BEReview(2,3,1,new DateTime()),
            new BEReview(2,3,2,new DateTime()),
            new BEReview(3,3,1,new DateTime()),
            new BEReview(4,3,0,new DateTime()),
            new BEReview(5,3,0,new DateTime())
        };

        //Creates a mockRepository and setting it up
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        //Initializes the service with the given mockRepository
        IReviewService service = new ReviewService(mockRepository.Object);

        //Act & Assert
        //Assert that the method will throw an InvalidOperationException when the given reviewer has no reviews yet
        Assert.Throws<InvalidOperationException>(() => service.GetTopMoviesByReviewer(reviewerWithNoReviews));
    }
    
    //Setting up the data to be tested afterwards (InLineData didn't allow the proper testing for DateTime, so 
    //I decided to use MemberData
    //Documentation at (https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/)
    
    public static readonly object[][] BeReviewsWithOnlyOneMovie =
    {
        //Each object will provide the data needed for each scenario
        //1st scenario
        new object[] { new int[] //reviewerId
        {
            1,2,2,3,3,4,4,5,5
        }, new int[] //MovieId
        {
            1,1,2,1,2,1,2,1,2
        },new int[] //ExpectedListToBeReturned
        {
            1
        },new DateTime[] //Movie dates
        {
            new DateTime(2010, 10, 2),
            new DateTime(2010, 10, 2),
            new DateTime(2008,10,30),
            new DateTime(2010, 10, 2),
            new DateTime(2008,10,30),
            new DateTime(2010, 10, 2),
            new DateTime(2008,10,30),
            new DateTime(2010, 10, 2),
            new DateTime(2008,10,30),
        }},
        //2nd scenario
        new object[] { new int[] //reviewerId
        {
            1,1,1,1,3,4,4,5,5
        }, new int[] //MovieId
        {
            1,2,3,4,1,1,2,1,2
        },new int[] //ExpectedListToBeReturned
        {
            3,2,4,1
        },new DateTime[] //Movie dates
        {
            new DateTime(2010, 10, 2),
            new DateTime(2012, 10, 2),
            new DateTime(2012,10,30),
            new DateTime(2012, 9, 2),
            new DateTime(2010, 10, 2),
            new DateTime(2010, 10, 2),
            new DateTime(2012, 10, 2),
            new DateTime(2010, 10, 2),
            new DateTime(2012, 10, 2),
        }},
    };
    
    [Theory, MemberData(nameof(BeReviewsWithOnlyOneMovie))]
    public void GetTopMoviesByReviewer(int[] reviewerId, int[] movie, int[]expected, DateTime[] date)
    {
        var theChoosenOne = 1; //
        //Initializes de repository
        var fakeRepo = new BEReview[]
        {
            new BEReview(reviewerId[0],movie[0],3,date[0]),
            new BEReview(reviewerId[1],movie[1],2,date[1]),
            new BEReview(reviewerId[2],movie[2],1,date[2]),
            new BEReview(reviewerId[3],movie[3],0,date[3]),
            new BEReview(reviewerId[4],movie[4],0,date[4]),
            new BEReview(reviewerId[5],movie[5],2,date[5]),
            new BEReview(reviewerId[6],movie[6],1,date[6]),
            new BEReview(reviewerId[7],movie[7],0,date[7]),
            new BEReview(reviewerId[8],movie[8],0,date[8])
        };

        //Creates a mockRepository and setting it up
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(fakeRepo);
        
        //Initializes the service with the given mockRepository
        IReviewService service = new ReviewService(mockRepository.Object);

        //Act
        var topMovies = service.GetTopMoviesByReviewer(theChoosenOne);
        
        //Assert
        
        Assert.Equal(expected,topMovies);
        mockRepository.Verify(r => r.GetAll(), Times.Once);
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