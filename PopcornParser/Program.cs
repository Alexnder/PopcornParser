using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Popcorn.ServiceLayer;
using Popcorn.Models.ParsingModels;

using LumenWorks.Framework.IO.Csv;

namespace PopcornParser
{
    class Program
    {

        public static CsvReader csv;

        public static List<Cinema> CinemaList = new List<Cinema>();

        public static string[] Halls = new string[] { "REGULAR", "VOX MAX", "VOX LD", "VOX GOLD", "IMAX", "The Picturehouse", "Platinum" };
            
        static void Main(string[] args)
        {
            
            SheduleParser parser;

            //Parse
            try
            {
                foreach (string FileName in Directory.GetFiles(@".", "*.csv", SearchOption.AllDirectories))
                    if (File.Exists(FileName))
                    {
                        csv = new CsvReader(new StreamReader(FileName), false);

                        //If delimiters are ";"
                        if (csv.FieldCount == 1)
                            csv = new CsvReader(new StreamReader(FileName), false, ';');

                        switch (csv.FieldCount)
                        {
                            case 9:
                                parser = new RoyalParser();
                                break;
                            //case 12:
                            //    parser = new GrandPoorParser();
                            //    break;
                            case 13:
                                parser = new GrandParser();
                                break;
                            case 15:
                                parser = new GrandParser();
                                break;
                            case 24:
                                parser = new VoxParser();
                                break;
                            default:
                                Console.WriteLine("Number of rows is:{0}\n This is error number and don't parse", csv.FieldCount);
                                continue;
                        }
                            
                        parser.parse(csv);
                    }
                    else
                        Console.WriteLine("File not exised");
            }
            catch
            {
                Console.WriteLine("One error occuared");
            }

            //Out parsed information into "OUT.txt"
            using (StreamWriter sw = new StreamWriter("OUT.txt"))
            {
                sw.WriteLine();
                sw.WriteLine("Parsed cinemas and movies:");
                foreach (Cinema cinema in CinemaList)
                {
                    sw.WriteLine("______________________________________");
                    sw.WriteLine(cinema.Name);
                    sw.WriteLine("--------------------------------------");
                    sw.WriteLine("{0,-28}|{1,-4}|{2,-4}", "Tittle", "Time", "Rate");
                    sw.WriteLine("--------------------------------------");

                    foreach (Movie movie in cinema.Movies)
                    {
                        sw.WriteLine("{0,-28} {1,-4} {2,-4}",movie.Tittle, movie.TimeInMinutes, movie.Rating);

                        foreach (SheduleNoteDate show in movie.SheduleNoteDates)
                        {
                            sw.WriteLine("{0,-20}; Hall: {1}", show.DateTimeStart, show.Hall);
                        }
                    }
                    sw.WriteLine();
                }
            }
           
            
        }
    }
}
