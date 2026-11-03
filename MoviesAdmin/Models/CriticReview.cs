namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        // primary key
        public int Id { get; set; }

        // foreign key
        public int MovieId { get; set; }

        // navigation property
        public Movie Movie { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public int Rating { get; set; }

        public bool IsPublished { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateOnly CreatedDate { get; set; }
    }
}
