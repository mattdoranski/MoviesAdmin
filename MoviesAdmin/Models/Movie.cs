using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Synopsis { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Genre {  get; set; } = string.Empty;

        [Required]
        [StringLength(5)] //Max length 5 chars (pg-13/nc-17)
        public string Rating {  get; set; } = string.Empty; // G, PG, PG-13, R, NC-17

        [Required]
        public int Runtime { get; set; } // Minutes

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateOnly ReleaseDate { get; set; }

        [Required]
        public string Director { get; set; } = string.Empty;

        public string RuntimeHours //Format runtime for hour/minute display
        {
            get
            {
                int hours = Runtime / 60;
                int mins = Runtime % 60;
                return $"{hours}h {mins}min";
            }
        }
    }
}
