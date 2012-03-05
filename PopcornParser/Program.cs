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
        public static string FileName; 

        public static CsvReader csv;

        public static List<Cinema> CinemaList = new List<Cinema>();

        public static string[] Halls = new string[] { "REGULAR", "VOX MAX", "VOX LD", "IMAX", "The Picturehouse", "Platinum movie suites" };
            
        static void Main(string[] args)
        {
            //string path = ""; //path to the source file

            SheduleParser parser;

            FileName = "test_grand.csv"; //TODO: Add cicle

            if (File.Exists(FileName))
            {
                csv = new CsvReader(new StreamReader(FileName), false);

                if (csv.FieldCount == 1)
                    csv = new CsvReader(new StreamReader(FileName), false, ';');

                Console.WriteLine("Number of rows is:{0}", csv.FieldCount);

                if (csv.FieldCount == 13 || csv.FieldCount == 15)
                {
                    parser = new GrandParser();
                    parser.parse(csv);
                }
            }
            else
                Console.WriteLine("File not exised");

            //Out parsed information
            Console.WriteLine();
            Console.WriteLine("Parsed cinemas and movies:");
            foreach (Cinema cinema in CinemaList)
            {
                Console.WriteLine("______________________________________");
                Console.WriteLine(cinema.Name);
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("{0,-28}|{1,-4}|{2,-4}", "Tittle", "Time", "Rate");
                Console.WriteLine("--------------------------------------");

                foreach (Movie movie in cinema.Movies)
                {
                    Console.WriteLine("{0,-28} {1,-4} {2,-4}",movie.Tittle, movie.TimeInMinutes, movie.Rating);

                    foreach (SheduleNoteDate show in movie.SheduleNoteDates)
                    {
                        Console.WriteLine("{0,-20}; Hall:{1}", show.DateTimeStart, show.Hall);
                    }
                }
                Console.WriteLine();
            }
           
            
        }
    }
}
