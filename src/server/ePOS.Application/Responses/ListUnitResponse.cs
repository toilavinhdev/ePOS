using ePOS.Application.ViewModels;
using ePOS.Shared.ValueObjects;

namespace ePOS.Application.Responses;

public class ListUnitResponse
{
    public List<UnitViewModel> Records { get; set; } = default!;

    public Paginator Paginator { get; set; } = default!;
}