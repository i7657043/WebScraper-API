namespace ScraperAPI.ErrorModels
{
    public class InnerError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public InnerError NextInnerError { get; set; }
    }
}
