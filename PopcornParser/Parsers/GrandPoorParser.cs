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
    class GrandPoorParser : GrandParser
    {
        public override void parse(CsvReader csv)
        {
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
                                if (GrandParseDate(csv[index], out StartDate))
                                {
                                    break;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        csv.ReadNextRecord();
                        if ((csv[0] == "CIN" || csv[0] == "CINE")) 
                            break;
                    }

                    //Parse Movies
                    while (csv.ReadNextRecord())
                    {
                        if (csv[0] != "" && csv[1] != "")
                        {
                            movie = new Movie();
                            movie.Tittle = csv[1];
                            //movie.TimeInMinutes = FieldsParser.ParseMovieDuration(csv[2]);
                            movie.Rating = csv[2];


                            for (int i = 3; i < csv.FieldCount; i++)
                            {
                                if (csv[i] != "")
                                {
                                    if ((index = FieldsParser.ParseMovieStartTime(csv[i], out CurrentDate)) >= 0)
                                    {
                                        //Add all days of week 
                                        for (double day = 0; day < 7; day++)
                                        {

                                            SheduleNoteDate NoteDate = new SheduleNoteDate();
                                            NoteDate.DateTimeStart = StartDate + new TimeSpan(CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second);

                                            if (CurrentDate.Hour < 3)
                                                NoteDate.DateTimeStart = NoteDate.DateTimeStart.AddDays(1);

                                            NoteDate.DateTimeStart = NoteDate.DateTimeStart.AddDays(day);

                                            NoteDate.Hall = FieldsParser.HallChoice(csv[0]);
                                        
                                            movie.SheduleNoteDates.Add(NoteDate);

                                        }
                                    }
                                    
                                }
                            }

                            cinema.Movies.Add(movie);
                        }
                    }  
                }
            }
            Program.CinemaList.Add(cinema);
        }
    }
}
