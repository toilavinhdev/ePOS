using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.TenantAggregate;

public class TenantTax : AuditableEntity
{
    public bool IsApplyForAllItem { get; set; }
    
    public bool IsPriceIncludeTax { get; set; }
    
    public int Value { get; set; }
    
    [ForeignKey(nameof(TenantId))]
    public Tenant Tenant { get; set; } = default!;
}