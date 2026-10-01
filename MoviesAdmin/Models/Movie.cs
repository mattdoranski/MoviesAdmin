using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide a title")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a synopsis")]
        [StringLength(500)]
        public string Synopsis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a genre")]
        [StringLength(100)]
        public string Genre {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a rating")]
        [RegularExpression("^(G|PG|PG-13|R|NC-17)$", ErrorMessage = "Rating must be G, PG, PG-13, R, or NC-17.")]
        public string Rating {  get; set; } = string.Empty; // G, PG, PG-13, R, NC-17

        [Required(ErrorMessage = "Please provide a runtime in minutes")]
        public int Runtime { get; set; } // Minutes

        [Required(ErrorMessage = "Please select a release date")]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateOnly ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please provide a director")]
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
