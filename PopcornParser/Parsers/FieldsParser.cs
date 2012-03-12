using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using LumenWorks.Framework.IO.Csv;
using Popcorn.Models.ParsingModels;

namespace Popcorn.ServiceLayer
{
    class FieldsParser
    {
        public static int OneFieldCheck(CsvReader csv)
        {
            /* 
             * If only one field filled in this csv array, then will be returned index of this field
             * Otherwise, -1 will be returned
             */

            string FieldName = "";

            int FieldIndex = -1;

            for (int i = 0; i < csv.FieldCount; i++)
                if (csv[i] != "")
                    if (FieldName == "")
                    {
                        FieldName = csv[i];

                        FieldIndex = i;
                    }

                    else
                        return -1;
            return FieldIndex;

        }

        public static bool RoyalParseDate(string TextToSplit, out DateTime Start)
        {
            /* 
             * It is parse time line in Royal's file? and save it in Start
             * Return true, if all goes well. false otherwise
             */
            MatchCollection matches = Regex.Matches(TextToSplit, "\\b(\\w+ \\d+)|\\b(\\d){4}$", RegexOptions.IgnoreCase);

            if (matches.Count == 3)
            {
                if (DateTime.TryParseExact(matches[0].Value + " " + matches[2].Value,
                    "MMM dd yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
                {
                    return true;
                }
            }
            Start = new DateTime();
            return false;
        }

        public static bool GrandParseDate(string TextToSplit, out DateTime Start)
        {
            /* 
             * It is parse time line in Grand's file? and save it in Start
             * Return true, if all goes well. false otherwise
             */
            if(TextToSplit[2] != ' ')
                TextToSplit = TextToSplit.Substring(0, 2) + TextToSplit.Substring(4);
            MatchCollection matches = Regex.Matches(TextToSplit, "^(\\d+ \\w+ \\d+)", RegexOptions.IgnoreCase);
            //if (matches.Count == 1)
            //{
            if (matches.Count == 0)
            {
                Start = new DateTime();
                return false;
            }
            if (DateTime.TryParseExact(matches[0].Value,
                "dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                Console.WriteLine("GrandParseDate Ok: {0}",Start.ToShortDateString());

                return true;
            }
            else
                Console.WriteLine("GrandParseDate: Before:{0} After:{1}", TextToSplit, matches[0].Value);
            //}

            return false;
        }


        public static bool VoxParseDate(string TextToSplit, out DateTime Start)
        {
            /* 
             * It is parse time line in Grand's file? and save it in Start
             * Return true, if all goes well. false otherwise
             */
            MatchCollection matches = Regex.Matches(TextToSplit, "^(\\d+ \\w+)\\b|\\b(\\d){4}$", RegexOptions.IgnoreCase);

            if (matches.Count < 2)
            {
                Start = new DateTime();
                return false;
            }

            if (DateTime.TryParseExact(matches[0].Value + " " + matches[1].Value,
                "dd MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                Console.WriteLine("GrandParseDate Ok: {0}", Start.ToShortDateString());

                return true;
            }
            else
                Console.WriteLine("GrandParseDate: Before:{0} After:{1}", TextToSplit, matches[0].Value + " " + matches[1].Value);

            return false;
        }


        public static int ParseMovieDuration(string TextFilmTime)
        {
            /* 
             * It is parse film time line
             * Return film time, if all goes well. 0 otherwise
             */

            DateTime FilmTime;
            // With AM/PM
            //Parse hh:mm full... 01:30 example
            if (DateTime.TryParseExact(TextFilmTime,
                "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }
            else if (DateTime.TryParseExact(TextFilmTime,
                "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }

            //Parse h:mm... 1:30 example
            if (DateTime.TryParseExact(TextFilmTime,
                "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }
            else if (DateTime.TryParseExact(TextFilmTime,
                "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }

            //Without AM/PM
            //Parse hh:mm full... 01:30 example
            if (DateTime.TryParseExact(TextFilmTime,
                "hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }
            else if (DateTime.TryParseExact(TextFilmTime,
                "hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }

            //Parse h:mm... 1:30 example
            if (DateTime.TryParseExact(TextFilmTime,
                "h:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }
            else if (DateTime.TryParseExact(TextFilmTime,
                "h:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FilmTime))
            {
                return FilmTime.Hour * 60 + FilmTime.Minute;
            }
            
            
            Console.WriteLine("Tryparse in ParseMovieDuration() failed!");

            return 0;
        }

        public static int ParseMovieStartTime(string TextToSplit, out DateTime Start)
        {
            /* 
             * It is parse film start time
             * Return true, if all goes well. false otherwise
             */
            if (DateTime.TryParseExact(TextToSplit,
                "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }
            else if (DateTime.TryParseExact(TextToSplit,
                "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }

            if (DateTime.TryParseExact(TextToSplit,
            "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }
            else if (DateTime.TryParseExact(TextToSplit,
            "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }

            if (TextToSplit.Length > 1)
            {
                if (TextToSplit.Substring(0,2) == "12") 
                    TextToSplit += " PM";
                else
                    TextToSplit += " AM";
            }

            if (DateTime.TryParseExact(TextToSplit,
            "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 0;
            }
            else if (DateTime.TryParseExact(TextToSplit,
            "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 0;
            }
            if (DateTime.TryParseExact(TextToSplit,
            "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 0;
            }
            else if (DateTime.TryParseExact(TextToSplit,
            "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 0;
            }
            //Console.WriteLine("Tryparse in ParseMovieStartTime() failed!");
            return -1;
        }

        //This need to DifferenceDayOfWeek()
        public static int DayOfWeekNumber(string TextDayPresent)
        {

            if (String.Compare(DayOfWeek.Monday.ToString(), TextDayPresent) == 0)
                return 1;
            if (String.Compare(DayOfWeek.Thursday.ToString(), TextDayPresent) == 0)
                return 2;
            if (String.Compare(DayOfWeek.Wednesday.ToString(), TextDayPresent) == 0)
                return 3;
            if (String.Compare(DayOfWeek.Thursday.ToString(), TextDayPresent) == 0)
                return 4;
            if (String.Compare(DayOfWeek.Friday.ToString(), TextDayPresent) == 0)
                return 5;
            if (String.Compare(DayOfWeek.Saturday.ToString(), TextDayPresent) == 0)
                return 6;
            if (String.Compare(DayOfWeek.Sunday.ToString(), TextDayPresent) == 0)
                return 7;
            return 0;
        }

        public static int DifferenceDayOfWeek(string TextToSplit)
        {
            /* 
             * It is parse film start time
             * Return true, if all goes well. false otherwise
             */
            
            int result;

            int increment;

            MatchCollection matches = Regex.Matches(TextToSplit, "^(\\w+)\\b|\\b(\\w+)$", RegexOptions.IgnoreCase);

            if (matches.Count == 2)
            {
                if ((increment = DayOfWeekNumber(matches[0].Value)) > 0)

                    if ((result = DayOfWeekNumber(matches[1].Value)) > 0)

                        return (result - increment + 6) % 7;
            }
            
            return -1;
        }

        public static int IsMidnight(int num)
        {
            if (num < 3)
                return 1;
            return 0;
        }

    }
}

