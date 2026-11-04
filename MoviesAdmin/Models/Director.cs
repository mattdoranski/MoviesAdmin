using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MoviesAdmin.Models
{
    public class Director
    {
        // primary key
        public int Id { get; set; }

        // naviagtion property
        [ValidateNever]
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
