namespace HotelShare.Web.Settings.API
{
    public class ApiAuthSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public int ExpirationTimeInSeconds { get; set; }
    }
}