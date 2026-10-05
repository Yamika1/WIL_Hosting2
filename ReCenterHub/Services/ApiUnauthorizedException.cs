namespace ReCenterHub.Services
{
    public class ApiUnauthorizedException : Exception
    {
        public ApiUnauthorizedException()
            : base("The API rejected the access token.")
        {
        }
    }
}