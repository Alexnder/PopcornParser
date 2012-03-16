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

        public static CsvReader Csv;

        public static List<Cinema> CinemaList = new List<Cinema>();

        public static string[] Halls = new string[] { "REGULAR", "VOX MAX", "VOX LD", "VOX GOLD", "IMAX", "The Picturehouse", "Platinum" };

        //public static string[] Cinemas = new string[] { "VOX", "GRAND", "ROYAL", "GALLERIA", "OSCAR", "BAWADI" };
        public enum Cinemalist { Vox, Grand, Royal, Galleria, Oscar, Bawadi };
            
        static void Main(string[] args)
        {
            //Parse
            try
            {
                foreach (string fileName in Directory.GetFiles(@".", "*.csv", SearchOption.AllDirectories))
                    if (File.Exists(fileName))
                    {
                        string cinemaName = ""; //Cinema identifer

                        try
                        {
                            //Finding cinema identifer
                            StreamReader streamInputCsv;
                            using (streamInputCsv = new StreamReader(fileName))
                            {
                                for (string line = streamInputCsv.ReadLine(); line != null; line = streamInputCsv.ReadLine())
                                {
                                    if ((cinemaName = FieldsParser.CinemaChoice(line)) != "")
                                        break;
                                }
                            }

                            //If identifer not found
                            if (cinemaName == "")
                                continue;

                            Csv = new CsvReader(new StreamReader(fileName), false);

                            //If delimiters are ";"
                            if (Csv.FieldCount == 1)
                                Csv = new CsvReader(new StreamReader(fileName), false, ';');
                        }
                        catch
                        {
                            Console.WriteLine("Parse identifer not found");
                        }

                        SheduleParser parser;

                        switch (cinemaName)
                        {
                            case "Royal":
                                parser = new RoyalParser();
                                break;

                            case "Grand":
                            case "Galleria":
                            case "Oscar":
                            case "Bawadi":
                                {
                                    if (Csv.FieldCount > 11 && Csv.FieldCount < 15)
                                    {
                                        parser = new GrandPoorParser();
                                        break;
                                    }
                                    else if (Csv.FieldCount > 14 && Csv.FieldCount < 18)
                                    {
                                        parser = new GrandParser();
                                        break;
                                    }
                                        Console.WriteLine("Grand Parser identifer is baad");
                                        continue;
                                }

                            case "Vox":
                                parser = new VoxParser();
                                break;

                            default:
                                Console.WriteLine("Parser identifer not found");
                                continue;
                        }

                        try
                        {
                            parser.Parse(Csv);

                            Csv.Dispose();
                        }
                        catch
                        {
                            Console.WriteLine("Parse error occuared");
                        }


                    }
                    else
                        Console.WriteLine("File not exised");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
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
