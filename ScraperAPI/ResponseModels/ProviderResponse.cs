namespace ScraperAPI.ResponseModels
{
    public class ProviderResponse<T> : ResponseBase<T>
    {
        public int ResponseCode { get; set; }
    }
}
