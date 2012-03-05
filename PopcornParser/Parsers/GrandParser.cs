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

        DateTime CurrentDate;

        Movie movie;

        Cinema cinema;

        public void parse(CsvReader csv)
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
                                if (FieldsParser.GrandParseDate(csv[index], out StartDate))
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
                        if (csv[1] != "" && csv[2] != "")
                        {
                            movie = new Movie();
                            movie.Tittle = csv[1];
                            movie.TimeInMinutes = FieldsParser.ParseMovieDuration(csv[2]);
                            movie.Rating = csv[3];

                            //Filling of SheduleNoteDates
                            int Factor = 0;
                            int LastHour = 0;
                            bool MidnightFlag = false;

                            for (int i = 4; i < csv.FieldCount; i++)
                            {
                                if (csv[i] != "")
                                {
                                    if ((index = FieldsParser.ParseMovieStartTime(csv[i], out CurrentDate)) == 0)
                                    {
                                        if ((MidnightFlag == true) && (CurrentDate.Hour == 12))
                                        {
                                            Factor++;
                                        }

                                        if (CurrentDate.Hour < LastHour)
                                        {
                                            
                                            MidnightFlag = true;
                                            Factor++;

                                        }
                                        //If the movie starts in the afternoon
                                        if (i == 6 && csv[5] == "" && CurrentDate.Hour < 6)
                                            Factor++;

                                        LastHour = CurrentDate.Hour;
                                        CurrentDate = CurrentDate.AddHours(Factor * 12);
                                        
                                    }
                                    if (index >= 0)
                                    {
                                        SheduleNoteDate NoteDate = new SheduleNoteDate();
                                        NoteDate.DateTimeStart = StartDate + new TimeSpan(CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second);
                                        
                                        if (Factor == 2)
                                            NoteDate.DateTimeStart = NoteDate.DateTimeStart.AddHours(12);
                                        if (csv[5] == "" && csv[6] == "")
                                            NoteDate.DateTimeStart = NoteDate.DateTimeStart.AddHours(12);
                                        if (Program.Halls.Contains(csv[0])) //TODO: Add IgnoreCase
                                            NoteDate.Hall = csv[0];
                                        movie.SheduleNoteDates.Add(NoteDate);
                                    }
                                    
                                }
                            }

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
