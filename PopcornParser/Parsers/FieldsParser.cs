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

            string fieldName = "";

            int fieldIndex = -1;

            for (int i = 0; i < csv.FieldCount; i++)
                if (csv[i] != "")
                    if (fieldName == "")
                    {
                        fieldName = csv[i];

                        fieldIndex = i;
                    }

                    else
                        return -1;
            return fieldIndex;

        }

        public static int ParseMovieDuration(string textFilmTime)
        {
            /* 
             * It is parse film time line
             * Return film time, if all goes well. 0 otherwise
             */

            DateTime filmTime;
            // With AM/PM
            //Parse hh:mm full... 01:30 example
            if (DateTime.TryParseExact(textFilmTime,
                "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }
            else if (DateTime.TryParseExact(textFilmTime,
                "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }

            //Parse h:mm... 1:30 example
            if (DateTime.TryParseExact(textFilmTime,
                "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }
            else if (DateTime.TryParseExact(textFilmTime,
                "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }

            //Without AM/PM
            //Parse hh:mm full... 01:30 example
            if (DateTime.TryParseExact(textFilmTime,
                "hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }
            else if (DateTime.TryParseExact(textFilmTime,
                "hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }

            //Parse h:mm... 1:30 example
            if (DateTime.TryParseExact(textFilmTime,
                "h:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
            }
            else if (DateTime.TryParseExact(textFilmTime,
                "h:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out filmTime))
            {
                return filmTime.Hour * 60 + filmTime.Minute;
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

            //It is to GrandPoorParser
            if (DateTime.TryParseExact(TextToSplit,
                "hh:mm:tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }
            else if (DateTime.TryParseExact(TextToSplit,
                "h:mm:tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }
            //It is to GrandPoorParser
            if (DateTime.TryParseExact(TextToSplit,
                "hh:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                return 1;
            }
            else if (DateTime.TryParseExact(TextToSplit,
                "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
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
        public static int DayOfWeekNumber(string textDayPresent)
        {

            if (String.Compare(DayOfWeek.Monday.ToString(), textDayPresent) == 0)
                return 1;
            if (String.Compare(DayOfWeek.Tuesday.ToString(), textDayPresent) == 0)
                return 2;
            if (String.Compare(DayOfWeek.Wednesday.ToString(), textDayPresent) == 0)
                return 3;
            if (String.Compare(DayOfWeek.Thursday.ToString(), textDayPresent) == 0)
                return 4;
            if (String.Compare(DayOfWeek.Friday.ToString(), textDayPresent) == 0)
                return 5;
            if (String.Compare(DayOfWeek.Saturday.ToString(), textDayPresent) == 0)
                return 6;
            if (String.Compare(DayOfWeek.Sunday.ToString(), textDayPresent) == 0)
                return 7;
            return 0;
        }

        public static int DifferenceDayOfWeek(string TextToSplit)
        {
            /* 
             * It is parse film start time
             * Return true, if all goes well. false otherwise
             */

            MatchCollection matches = Regex.Matches(TextToSplit, "^(\\w+)\\b|\\b(\\w+)$", RegexOptions.IgnoreCase);

            if (matches.Count == 2)
            {
                int increment;
                if ((increment = DayOfWeekNumber(matches[0].Value)) > 0)

                {
                    int result;
                    if ((result = DayOfWeekNumber(matches[1].Value)) > 0)

                        return (result - increment + 7) % 7;
                }
            }

            if (matches.Count == 1)
            {
                return 0;
            }
            
            return -1;
        }

        public static int IsMidnight(int num)
        {
            if (num < 3)
                return 1;
            return 0;
        }

        public static string HallChoice(string possibleHallName)
        {
            foreach (string hallName in PopcornParser.Program.Halls)
            {
                for (int i=0; i <= possibleHallName.Length - hallName.Length; i++)
                    if (string.Compare(possibleHallName, i, hallName, 0, hallName.Length, true) == 0)
                        return hallName;
            }
            return "";
        }

        public static string CinemaChoice(string possibleCinemaName)
        {
            foreach (PopcornParser.Program.Cinemalist cinemaName in Enum.GetValues(typeof(PopcornParser.Program.Cinemalist)))
            {
                for (int i = 0; i <= possibleCinemaName.Length - cinemaName.ToString().Length; i++)
                    if (string.Compare(possibleCinemaName, i, cinemaName.ToString(), 0, cinemaName.ToString().Length, true) == 0)
                        return cinemaName.ToString();
            }
            return "";
        }
    }
}

