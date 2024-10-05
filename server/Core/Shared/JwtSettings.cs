namespace Core.Options;

public class JwtSettings
{
    public const string Section = nameof(JwtSettings);

    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public int TokenExpirationInMinutes { get; set; }
}
