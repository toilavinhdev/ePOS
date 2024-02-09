using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.OptionAttributeAggregate;

public class OptionAttributeValue : Entity
{
    public string Name { get; set; } = default!;
    
    public Guid OptionAttributeId { get; set; }
    [ForeignKey(nameof(OptionAttributeId))]
    public OptionAttribute OptionAttribute { get; set; } = default!;
    
    public int SortIndex { get; set; }
}