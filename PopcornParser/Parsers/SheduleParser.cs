using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popcorn.Models.ParsingModels;
using LumenWorks.Framework.IO.Csv;

namespace Popcorn.ServiceLayer
{
    public abstract class SheduleParser
    {
        public DateTime StartDate;

        public DateTime CurrentDate;

        public Movie movie;

        public Cinema cinema;

        public abstract void Parse(CsvReader csv);
    }
}
