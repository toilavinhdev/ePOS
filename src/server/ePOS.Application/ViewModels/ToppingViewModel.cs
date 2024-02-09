namespace ePOS.Application.ViewModels;

public class ToppingViewModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public double Price { get; set; }
    
    public bool IsActive { get; set; }
    
    public int ItemCount { get; set; }
}