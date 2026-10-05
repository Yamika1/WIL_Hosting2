namespace ReCenterHub.Services
{
    public record RegisterResult(bool Succeeded, IEnumerable<string> Errors);
}