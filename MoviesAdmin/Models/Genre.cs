using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        //Primary key
        public int Id { get; set; }

        // Navigation property
        [ValidateNever]
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public string Title { get; set; } = string.Empty;
    }
}
