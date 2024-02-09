using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.TenantAggregate;

public class Tenant : IAggregateRoot
{
    public Guid Id { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public string? LogoUrl { get; set; }
    
    public string? TaxId { get; set; }
    
    public string? CompanyName { get; set; }
    
    public string? CompanyAddress { get; set; }

    public TenantTax TenantTax { get; set; } = default!;
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
}