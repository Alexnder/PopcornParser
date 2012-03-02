using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using LumenWorks.Framework.IO.Csv;


namespace Popcorn.ServiceLayer
{
    class FieldsParser
    {
        public static int one_field_parse(CsvReader csv)
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
            MatchCollection matches = Regex.Matches(TextToSplit, "\\b(\\w+ \\d+)\\b|\\b(\\d){4}$", RegexOptions.IgnoreCase);

            if (matches.Count == 2)
            {
                if (DateTime.TryParseExact(matches[0].Value + " " + matches[1].Value,
                    "MMM dd yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
                {
                    Console.WriteLine(Start.ToShortDateString());

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
            MatchCollection matches = Regex.Matches(TextToSplit, "^(\\w+ \\w+ \\d+)", RegexOptions.IgnoreCase);
            Console.WriteLine("GrandParseDate start:");
            Console.WriteLine(TextToSplit);
            //if (matches.Count == 1)
            //{
            if (DateTime.TryParseExact(matches[0].Value,
                "dd\\t\\h MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Start))
            {
                Console.WriteLine(Start.ToShortDateString());

                return true;
            }
            else
                Console.WriteLine(matches[0].Value);
            //}

            return false;
        }
    }
}

