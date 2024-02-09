namespace ePOS.Application.ViewModels;

public class OptionAttributeViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public bool IsActive { get; set; }

    public string[] Attributes { get; set; } = default!;
    
    public int ItemCount { get; set; }
}