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

        static void Main(string[] args)
        {
            //string path = ""; //path to the source file

            SheduleParser parser;
            if (File.Exists("test1.csv"))
            {
                csv = new CsvReader(new StreamReader(@"test2.csv"), false);

                Console.WriteLine("Number of rows is:{0}", csv.FieldCount);

                if (csv.FieldCount == 13)
                {
                    parser = new GrandParser();
                    parser.parse(csv);
                }
            }
            //Console.WriteLine(CinemaList[0].Name);
            
        }
    }
}
