using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Popcorn.ServiceLayer
{
    class FieldsParser
    {
        public static int one_field_parse(CsvReader csv)
        {
            /* 
             * If only one field filled in this csv array, then will be returned index of this field
             * Otherwise, -1 will be returned
             */

            string FieldName = "";

            int FieldIndex = -1;

            for (int i = 0; i < csv.FieldCount; i++)
                if (csv[i] != "")
                    if (FieldName == "")
                    {
                        FieldName = csv[i];

                        FieldIndex = i;
                    }

                    else
                        return -1;
            return FieldIndex;

        }
    }
}
