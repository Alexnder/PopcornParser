using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popcorn.ServiceLayer;
using Popcorn.Models.ParsingModels;
using PopcornParser;
using LumenWorks.Framework.IO.Csv;

namespace Popcorn.ServiceLayer
{
    class GrandParser : SheduleParser
    {
        DateTime StartDate;

        Movie movie;

        Cinema cinema;

        public void parse(CsvReader csv)
        {
            while (csv.ReadNextRecord())
            {
                int index = FieldsParser.one_field_parse(csv);
                if (index > -1)
                {
                    if (cinema == null)
                    {
                        cinema = new Cinema();
                        cinema.Name = csv[index];
                        cinema.Movies = new List<Movie>();
                    }
                    csv.ReadNextRecord();
                    if ((index = FieldsParser.one_field_parse(csv)) > 0)
                        FieldsParser.GrandParseDate(csv[index],out StartDate);

                    do csv.ReadNextRecord();
                    while ((csv[0] != "CIN" && csv[0] != "CINE"));

                    //Parse Movies
                    while (csv.ReadNextRecord())
                    {
                        if (csv[1] != "" && csv[2] != "")
                        {
                            movie = new Movie();
                            movie.Tittle = csv[1];
                            movie.TimeInMinutes = 0;    //TODO: this
                            movie.Rating = csv[3];
                            movie.NowShowing = true;
                            Console.WriteLine("{0} {1} {2}", csv[1], csv[2], csv[3]);
                            cinema.Movies.Add(movie);
                        }
                        if ((index = FieldsParser.one_field_parse(csv)) > 0)
                        {
                            Program.CinemaList.Add(cinema);
                            cinema = new Cinema();
                            cinema.Name = csv[index];
                            cinema.Movies = new List<Movie>();
                            break;
                        }
                    }  
                }
            }
            Program.CinemaList.Add(cinema);
        }
    }
}
