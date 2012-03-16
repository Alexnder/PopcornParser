using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Popcorn.ServiceLayer;
using Popcorn.Models.ParsingModels;
using PopcornParser;
using LumenWorks.Framework.IO.Csv;

namespace Popcorn.ServiceLayer
{
            
    public class VoxParser : SheduleParser
    {
       
        protected static bool VoxParseDate(string TextToSplit, out DateTime Start)
        {
            /* 
             * It is parse time line in Grand's file? and save it in Start
             * Return true, if all goes well. false otherwise
             */
            MatchCollection matches = Regex.Matches(TextToSplit, "^(\\d+ \\w+)\\b|\\b(\\d){4}$", RegexOptions.IgnoreCase);

            if (matches.Count < 2)
            {
                Start = new DateTime();
                Console.WriteLine("VoxParseDate() crash");
                return false;
            }

            if (DateTime.TryParseExact(matches[0].Value + " " + matches[1].Value,
                "dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return true;
            }
            else
                Console.WriteLine("VoxParseDate: Before:{0} After:{1}", TextToSplit, matches[0].Value + " " + matches[1].Value);

            return false;
        }


        public override void parse(CsvReader csv)
        {
            int DayCount = -1; //Counter of first row days in VOX file.. Some "Thursday to Saturday" = 3

            int index = 0;

            string LastFilm = "";

            string CurrentHall = "";

            int HallCredit = 0;
            
            //Parse cinema
            for (int i = 0; i < 8; i++)
            {
                csv.ReadNextRecord();

                index = FieldsParser.OneFieldCheck(csv);
                if (index > -1)
                {
                    cinema = new Cinema();
                    cinema.Name = csv[index];
                    break;
                }
            }


            //Parse date
            for (int i = 0; i < 5; i++)
            {
                csv.ReadNextRecord();
                if ((index = FieldsParser.OneFieldCheck(csv)) >= 0)
                {
                    if (VoxParseDate(csv[index], out StartDate))
                    {
                        break;
                    }
                }
            }
            

            //Parse day of weeks
            for (int i = 0; i < 2; i++)
            {
                csv.ReadNextRecord();
                for (int j = 0; j < csv.FieldCount; j++)
                {
                    DayCount = FieldsParser.DifferenceDayOfWeek(csv[j]);
                    if (DayCount >= 0)
                        break;
                }
                if (DayCount >= 0)
                        break;
            }

            //Skip empty lines
            for (int i = 0; i < 2; i++)
            {
                csv.ReadNextRecord();
                if (String.Compare("Cinema", csv[0], true) == 0)
                    break;
            }

            //Parse movies
            while (csv.ReadNextRecord())
            {
                for (int i = 0; i <= 1; i++)
                {
                    if (csv[1 + i * 6] != "" && csv[2 + i * 6] != "")
                    {
                        
                        if (LastFilm != csv[1 + i * 6])
                        {
                            //This Movie is not
                            if (!cinema.Movies.Exists(temp_cinema => temp_cinema.Tittle == csv[1 + i * 6]))
                            {
                                movie = new Movie();

                                movie.Tittle = csv[1 + i * 6];

                                movie.Rating = csv[4 + i * 6];

                                movie.TimeInMinutes = (FieldsParser.ParseMovieDuration(csv[3 + i * 6]) - FieldsParser.ParseMovieDuration(csv[2 + i * 6]) + 1440) % 1440;

                                cinema.Movies.Add(movie);
                            }
                            //This Movie is exist
                            else
                            {
                                movie = cinema.Movies.Find(temp_cinema => temp_cinema.Tittle == csv[1 + i * 6]);
                            }

                            if (HallCredit <= 0)
                                CurrentHall = "";

                        }

                       

                        if (CurrentHall == "")
                        {
                            CurrentHall = FieldsParser.HallChoice(csv[0]);

                            if (CurrentHall != "")
                                HallCredit = 8;
                        }

                        

                        // Parse SheduleNoteDates
                        for (int k = (DayCount + 1) * i; k <= DayCount + i * (6 - DayCount); k++)
                        {
                            if ((FieldsParser.ParseMovieStartTime(csv[2 + i * 6], out CurrentDate)) == 1)
                            {
                                SheduleNoteDate NoteDate = new SheduleNoteDate();
                                NoteDate.DateTimeStart = StartDate.AddHours(CurrentDate.Hour).AddMinutes(CurrentDate.Minute).AddDays(k + FieldsParser.IsMidnight(CurrentDate.Hour));
                                NoteDate.Hall = CurrentHall;
                                movie.SheduleNoteDates.Add(NoteDate);

                            }
                        }

                        LastFilm = csv[1 + i * 6];
                    }
                }

                HallCredit--;
            }

            Program.CinemaList.Add(cinema);

        }
    }
     
}
