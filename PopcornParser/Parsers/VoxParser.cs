using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popcorn.ServiceLayer;
using Popcorn.Models.ParsingModels;

namespace Popcorn.ServiceLayer
{
    public class VoxParser : SheduleParser
    {
        public Cinema parse(string[] lines)
        {
            Cinema cinema = new Cinema();

            //TODO: do parsing here
            
            foreach (var line in lines)
            {
                //cells is string array of real cells values
                var cells = line.Split(';');
            }

            return cinema;
        }
    }
}
