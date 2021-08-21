using System;

namespace imdbLookup
{

    public class Serie
    {
        public string Title { get; set; }
        public string imdbID { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Writer { get; set; }
        public string Production { get; set; }
        public string Genre { get; set; }
        public string Actors { get; set; }
        public string Language { get; set; }
        //public string Ratings { get; set;} //Ratings":[{"Source":"Internet Movie Database","Value":"7.8/10"}]

        //public string Plot { get; set; } cut text at sensible place if too long

    }

    public class ResError
    {
        public string Response { get; set; }
        public string Error { get; set; }

    }
}