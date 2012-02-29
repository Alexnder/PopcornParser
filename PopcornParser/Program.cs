using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Popcorn.ServiceLayer;
using Popcorn.Models.ParsingModels;


namespace PopcornParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ""; //path to the source file

            var lines = File.ReadAllLines(path);

            SheduleParser parser;
            
            parser = new VoxParser();

            var res = parser.parse(lines);
        }
    }
}
