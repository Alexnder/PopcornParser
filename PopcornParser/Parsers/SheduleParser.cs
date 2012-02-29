using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popcorn.Models.ParsingModels;

namespace Popcorn.ServiceLayer
{
    interface SheduleParser
    {
        Cinema parse(string[] lines);
    }
}
