using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

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

        //TODO: Biography property for director with functionality to display in details view for all users

        [Display(Name = "Picture")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
