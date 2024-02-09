namespace ePOS.Application.ViewModels;

public class TenantViewModel
{
    public Guid Id { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public string? LogoUrl { get; set; }
    
    public string? TaxId { get; set; }
    
    public string? CompanyName { get; set; }
    
    public string? CompanyAddress { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
}