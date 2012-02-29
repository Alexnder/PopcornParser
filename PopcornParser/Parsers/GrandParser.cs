using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Popcorn.ServiceLayer;
using Popcorn.Models.ParsingModels;
using PopcornParser;
using LumenWorks.Framework.IO.Csv;

namespace Popcorn.ServiceLayer
{
    class GrandParser : SheduleParser
    {
        bool datetime_parse(string TimeString)
        {
            //TODO: This
            //TimeString.match()
            try
            {
                DateTime.TryParse(TimeString, out StartDate);
                DateTime.TryParse(TimeString, out StartDate);
            }
            catch
            {
                return false;
            }
            return true;
        }

        DateTime StartDate;
        DateTime EndDate;

        public void parse(CsvReader csv)
        {
            while (csv.ReadNextRecord())
            {
                int index = FieldsParser.one_field_parse(csv);
                if (index > -1)
                {
                    Cinema cinema = new Cinema();
                    cinema.Name = csv[index];
                    csv.ReadNextRecord();
                    //datetime_parse(csv[FieldsParser.one_field_parse(csv)]);

                    do csv.ReadNextRecord();
                    while ((csv[0] != "CIN" && csv[0] != "CINE"));

                    //Parse Movies
                    while (csv.ReadNextRecord())
                    {
                        Console.WriteLine(csv[1]);
                        if (FieldsParser.one_field_parse(csv) > 0 )
                            break;
                    }

                    
                }



                //for (int i = 0; i < csv.FieldCount; i++)
                //    Console.Write(csv[i]);
                //Console.WriteLine();
            }
        }
    }
}
