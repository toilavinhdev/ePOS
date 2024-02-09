namespace ePOS.Application.ViewModels;

public class CategoryViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public bool IsActive { get; set; }
    
    public int ItemCount { get; set; }
}