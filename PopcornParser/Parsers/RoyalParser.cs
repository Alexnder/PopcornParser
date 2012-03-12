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
    class RoyalParser : SheduleParser
    {
        public override void parse(CsvReader csv)
        {
            string CurrentHall = "";

            while (csv.ReadNextRecord())
            {
                //Rewind blank lines to find Cinema name
                int index = FieldsParser.OneFieldCheck(csv);
                if (index > -1)
                {
                    if (cinema == null)
                    {
                        cinema = new Cinema();
                        cinema.Name = csv[index];
                    

                        //Rewind blank lines to find date
                        for (int i = 0; i<5; i++)
                        {
                            csv.ReadNextRecord();
                            if ((index = FieldsParser.OneFieldCheck(csv)) >= 0)
                            {
                                if (FieldsParser.RoyalParseDate(csv[index], out StartDate))
                                {
                                    break;
                                }
                            }
                        }
                    }

                    //Parse Movies
                    while (csv.ReadNextRecord())
                    {
                        if (csv[0] != "" && csv[1] == "" && csv[2] != "")
                        {
                            movie = new Movie();

                            //Hall choice
                            CurrentHall = FieldsParser.HallChoice(csv[0]);

                            csv.ReadNextRecord();

                            movie.Tittle = csv[0];
                            
                            //Filling of SheduleNoteDates
                            for (int k = 0; k < 7; k++)
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    if ((FieldsParser.ParseMovieStartTime(csv[k + 2], out CurrentDate)) == 1)
                                    {
                                        SheduleNoteDate NoteDate = new SheduleNoteDate();
                                        NoteDate.DateTimeStart = StartDate.AddHours(CurrentDate.Hour).AddMinutes(CurrentDate.Minute).AddDays(i + FieldsParser.IsMidnight(CurrentDate.Hour));
                                        NoteDate.Hall = CurrentHall;
                                        movie.SheduleNoteDates.Add(NoteDate);
                                    }
                                }
                            }
                            //Skip not needed lines
                            csv.ReadNextRecord();
                            csv.ReadNextRecord();
                            csv.ReadNextRecord();

                            //Parse movie duration
                            for (int k = 2; k < csv.FieldCount; k++)
                            {
                                if (csv[k] == "")
                                    continue;

                                movie.TimeInMinutes = FieldsParser.ParseMovieDuration(csv[k]);

                                if (movie.TimeInMinutes != 0)
                                    break;

                            }
                            //Parse rating
                            movie.Rating = csv[0];

                            cinema.Movies.Add(movie);
                        }
                        if ((index = FieldsParser.OneFieldCheck(csv)) > 0)
                        {
                            Program.CinemaList.Add(cinema);
                            cinema = new Cinema();
                            cinema.Name = csv[index];
                            break;
                        }
                    }
                }
            }
            Program.CinemaList.Add(cinema);
        }
    }
}
