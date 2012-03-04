using System;

namespace Popcorn.Models.ParsingModels
{
    public class SheduleNoteDate
    {
        public DateTime DateTimeStart { get; set; }

        public string Hall { get; set; }

        public SheduleNoteDate()
        {

            DateTimeStart = new DateTime();

            Hall = "";

        }
    }
}