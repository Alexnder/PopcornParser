using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popcorn.Models.ParsingModels;
using LumenWorks.Framework.IO.Csv;

namespace Popcorn.ServiceLayer
{
    interface SheduleParser
    {
        void parse(CsvReader csv);
    }
}
