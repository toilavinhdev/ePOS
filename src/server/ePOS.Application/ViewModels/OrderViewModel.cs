namespace ePOS.Application.ViewModels;

public class OrderViewModel
{
    public Guid Id { get; set; }
    
    public string? Description { get; set; }

    public double SubTotal { get; set; }
    
    public double TotalTax { get; set; }

    public double Total => SubTotal - TotalTax;

    public List<OrderItemViewModel> OrderItems { get; set; } = default!;
}

public class OrderItemViewModel
{
    public Guid ItemId { get; set; }
    
    public string ItemName { get; set; } = default!;

    public string? SizeName { get; set; }
    
    public int Quantity { get; set; }
    
    public double TotalLine { get; set; }
}