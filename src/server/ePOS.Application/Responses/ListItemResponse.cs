using ePOS.Application.ViewModels;
using ePOS.Shared.ValueObjects;

namespace ePOS.Application.Responses;

public class ListItemResponse
{
    public List<ItemViewModel> Records { get; set; } = default!;

    public Paginator Paginator { get; set; } = default!;
}
