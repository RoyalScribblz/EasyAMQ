namespace EasyAMQ.Options;

public class EasyAmqOptions
{
    public Uri ConnectionString { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}