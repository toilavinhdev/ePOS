using ePOS.Application.ViewModels;
using ePOS.Shared.ValueObjects;

namespace ePOS.Application.Responses;

public class ListCategoryResponse
{
    public List<CategoryViewModel> Records { get; set; } = default!;

    public int TotalRecords { get; set; } 

    public Paginator Paginator { get; set; } = default!;
}