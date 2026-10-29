using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        // Primary key
        public int Id { get; set; }

        //Foreign key to Genre
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        // Foreign key to AudienceRating
        [Display(Name = "Rating")]
        public int AudienceRatingId { get; set; }

        // Navigation property
        [ValidateNever]
        public Genre Genre { get; set; } = null!;

        // Navigation property
        [ValidateNever]
        public AudienceRating AudienceRating { get; set; } = null!;

        [Required(ErrorMessage = "Please provide a title")]
        [StringLength(100, ErrorMessage = "Maximum title character length is 100")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a synopsis")]
        [StringLength(500, ErrorMessage = "Maximum sysnopsis character length is 500")]
        public string Synopsis { get; set; } = string.Empty;

        //[MinLength(1, ErrorMessage = "Please provide a genre")]
        //public List<string> Genre { get; set; } = new();

        //[Required(ErrorMessage = "Please select a rating")]
        //public string Rating { get; set; } = string.Empty; // MPA film rating

        [Required(ErrorMessage = "Please provide a runtime in minutes")]
        [Range(1,900, ErrorMessage = "Runtime must be between 1 and 900 minutes")] // 900 is just over longest listed movie on wikipedia (Resan)
        public int Runtime { get; set; } // Minutes

        [Required(ErrorMessage = "Please select a release date")]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)] // Format as calender in create/edit view
        public DateOnly ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please provide a director")]
        [StringLength(100, ErrorMessage = "Maximum name character length is 100")]
        public string Director { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [NotMapped]
        public string RuntimeHours //Format runtime for hour/minute display //DONT MIGRATE
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
