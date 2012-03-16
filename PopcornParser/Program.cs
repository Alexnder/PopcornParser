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

        //public static string[] Cinemas = new string[] { "VOX", "GRAND", "ROYAL", "GALLERIA CINEMA", "OSCAR CINEMAS", "BAWADI CINEMAS" };
        public enum cinemalist { VOX, GRAND, ROYAL, GALLERIA, OSCAR, BAWADI };
            
        static void Main(string[] args)
        {
            
            SheduleParser parser; 

            StreamReader StreamInputCSV;


            //Parse
            try
            {
                foreach (string FileName in Directory.GetFiles(@".", "*.csv", SearchOption.AllDirectories))
                    if (File.Exists(FileName))
                    {
                        
                        string CinemaName = ""; //Cinema identifer

                        try
                        {
                            //Finding cinema identifer
                            using (StreamInputCSV = new StreamReader(FileName))
                            {
                                for (string line = StreamInputCSV.ReadLine(); line != null; line = StreamInputCSV.ReadLine())
                                {
                                    if ((CinemaName = FieldsParser.CinemaChoice(line)) != "")
                                        break;
                                }
                            }

                            //If identifer not found
                            if (CinemaName == "")
                                continue;

                            csv = new CsvReader(new StreamReader(FileName), false);

                            //If delimiters are ";"
                            if (csv.FieldCount == 1)
                                csv = new CsvReader(new StreamReader(FileName), false, ';');
                        }
                        catch
                        {
                            Console.WriteLine("Parse identifer not found");
                        }

                        switch (CinemaName)
                        {
                            case "ROYAL":
                                parser = new RoyalParser();
                                break;

                            case "GRAND":
                            case "GALLERIA":
                            case "OSCAR":
                            case "BAWADI":
                                {
                                    if (csv.FieldCount > 11 && csv.FieldCount < 15)
                                    {
                                        parser = new GrandPoorParser();
                                        break;
                                    }
                                    else if (csv.FieldCount > 14 && csv.FieldCount < 18)
                                    {
                                        parser = new GrandParser();
                                        break;
                                    }
                                        Console.WriteLine("Grand Parser identifer is baad");
                                        continue;
                                }

                            case "VOX":
                                parser = new VoxParser();
                                break;

                            default:
                                Console.WriteLine("Parser identifer not found");
                                continue;
                        }

                        try
                        {
                            parser.parse(csv);

                            csv.Dispose();
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
