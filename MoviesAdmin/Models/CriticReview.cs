using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        // primary key
        public int Id { get; set; }

        // foreign key
        public int MovieId { get; set; }

        // navigation property
        [ValidateNever]
        public Movie Movie { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Maximum review character length is 1000")]
        [Display(Name = "Review")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        [Display(Name = "Rating (1-10)")]
        public int Rating { get; set; }

        [Display(Name = "Published")]
        public bool IsPublished { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateOnly CreatedDate { get; set; }
    }
}
