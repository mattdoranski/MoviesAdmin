namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        public int ID { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Rating { get; set; }

        public bool IsPublished { get; set; }

        public string CreatedBy { get; set; } = string.Empty

        public DateOnly CreatedDate { get; set; }
    }
}
