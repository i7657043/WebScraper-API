namespace ScraperAPI.ErrorModels
{
    public class Error
    {
        //One of a server-defined set of error codes
        public int Code { get; set; }

        //A human-readable representation of the error
        public string Message { get; set; }

        //The Target of the error
        public string Target { get; set; }

        public InnerError InnerError { get; set; }
    }
}
