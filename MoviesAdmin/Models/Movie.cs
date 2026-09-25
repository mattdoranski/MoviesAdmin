namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int ID { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Synopsis { get; set; } = string.Empty;

        public string Genre {  get; set; } = string.Empty;

        public string Rating {  get; set; } = string.Empty; // G, PG, PG-13, R, NC-17

        public int Runtime { get; set; } // Minutes

        public DateOnly ReleaseDate { get; set; }

        public string Director { get; set; } = string.Empty;
    }
}
