using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ItemAggregate;

public class ItemImage : Entity
{
    public string Url { get; set; } = default!;
    
    public int SortIndex { get; set; }

    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; } = default!;
}