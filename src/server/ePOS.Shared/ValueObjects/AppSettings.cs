namespace ePOS.Shared.ValueObjects;

public class AppSettings
{
    public string Host { get; set; } = default!;
    
    public ConnectionStrings ConnectionStrings { get; set; } = default!;
    
    public JwtTokenConfig JwtTokenConfig { get; set; } = default!;
    
    public StorageConfig StorageConfig { get; set; } = default!;
    
    public MailConfig MailConfig { get; set; } = default!;
}

public class ConnectionStrings
{
    public string SqlServerConnection { get; set; } = default!;
}

public class JwtTokenConfig
{
    public string ServerSecretKey { get; set; } = default!;
    
    public int AccessTokenExpirationMinutes { get; set; }
    
    public int RefreshTokenExpirationMinutes { get; set; }
}

public class StorageConfig
{
    public string Location { get; set; } = default!;

    public string ExternalPath { get; set; } = default!;
}

public class MailConfig
{
    public string Host { get; set; } = default!;
    
    public int Port { get; set; }
    
    public string DisplayName { get; set; } = default!;
    
    public string Mail { get; set; } = default!;
    
    public string Password { get; set; } = default!;
}