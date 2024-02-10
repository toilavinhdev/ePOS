﻿namespace ePOS.Application.ViewModels;

public class ItemViewModel
{
    public Guid Id { get; set; }
    
    public string Sku { get; set; } = default!;
    
    public string Name { get; set; } = default!;
    
    public bool IsActive { get; set; }
    
    public double? Price { get; set; }
    
    public List<ItemSizePriceViewModel>? SizePrices { get; set; }
    
    public Guid UnitId { get; set; }
    
    public string UnitName { get; set; } = default!;
    
    public List<ItemImageViewModel>? Images { get; set; } 
    
    public List<OptionAttributeViewModel>? OptionAttributes { get; set; }
    
    public List<ToppingViewModel>? Toppings { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
}

public class ItemImageViewModel
{
    public string Url { get; set; } = default!;
    
    public int SortIndex { get; set; }
}

public class ItemSizePriceViewModel
{
    public string Name { get; set; } = default!;
    
    public double Price { get; set; }
}