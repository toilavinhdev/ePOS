using System.Text.Json.Serialization;

namespace ePOS.Shared.ValueObjects;

public class Paginator
{
    [JsonPropertyName("pageIndex")]
    public int PageIndex { get; set; }
    
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
    
    [JsonPropertyName("totalRecords")]
    public int TotalRecords { get; set; }
        
    [JsonPropertyName("totalPages")]
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage => PageIndex > 1;

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage => PageIndex < TotalPages;
    
    public Paginator(int pageIndex, int pageSize, int totalRecords)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }
}