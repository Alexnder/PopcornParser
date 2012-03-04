using System;
using System.Collections.Generic;

namespace Popcorn.Models.ParsingModels
{
    public class Movie
    {
        public string Tittle { get; set; }

        public string Rating { get; set; }

        public bool NowShowing { get; set; }

        public int TimeInMinutes { get; set; }

        public List<SheduleNoteDate> SheduleNoteDates { get; set; }

        public Movie()
        {

            Tittle = "";

            Rating = "";

            NowShowing = true;

            TimeInMinutes = 0;

            SheduleNoteDates = new List<SheduleNoteDate>();

        }

    }
}