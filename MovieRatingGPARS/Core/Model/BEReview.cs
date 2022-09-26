using MovieRatingGPARS.Core.Repository;

namespace MovieRatingGPARS.Core.Model;

public class BEReview
{
    public int Reviewer { get; set; }
    public int Movie { get; set; }
    public int Grade { get; set; }
    public DateTime ReviewDate { get; set; }
    
}