using System.Text.Json.Serialization;

namespace ePOS.Shared.ValueObjects;

public class UserClaimsValue
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }
    
    [JsonPropertyName("tenantId")] 
    public Guid TenantId { get; set; }

    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}