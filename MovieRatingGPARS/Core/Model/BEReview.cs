using MovieRatingGPARS.Core.Repository;

namespace MovieRatingGPARS.Core.Model;

public class BEReview
{
    public int Reviewer { get; set; }
    public int Movie { get; set; }
    public int Grade { get; set; }
    public DateTime ReviewDate { get; set; }

    public BEReview(int reviewer, int movie, int grade, DateTime reviewDate)
    {
        Reviewer = reviewer;
        Movie = movie;
        Grade = grade;
        ReviewDate = reviewDate;
    }

    public BEReview()
    {
    }
}