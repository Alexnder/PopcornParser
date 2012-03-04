using System.Collections.Generic;

namespace Popcorn.Models.ParsingModels
{
    public class Cinema
    {
        public string Name { get; set; }

        public List<Movie> Movies { get; set; }

        public Cinema()
        {

            Name = "";

            Movies = new List<Movie>();

        }
    }
}
