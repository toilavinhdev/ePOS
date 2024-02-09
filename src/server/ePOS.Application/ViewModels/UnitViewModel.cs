namespace ePOS.Application.ViewModels;

public class UnitViewModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public bool IsDefault { get; set; }
    
    public int ItemCount { get; set; }
}