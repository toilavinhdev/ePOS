using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.FileAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Domain.OptionAttributeAggregate;
using ePOS.Domain.TenantAggregate;
using ePOS.Domain.ToppingAggregate;
using ePOS.Domain.UnitAggregate;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Common.Contracts;

public interface ITenantContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ());
    DbSet<ApplicationFile> Files { get; set; }
    DbSet<Tenant> Tenants { get; set; }
    DbSet<TenantTax> TenantTaxes { get; set; }
    DbSet<Unit> Units { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<CategoryItem> CategoryItems { get; set; }
    DbSet<Item> Items { get; set; }
    DbSet<ItemImage> ItemImages { get; set; }
    DbSet<ItemOptionAttribute> ItemOptionAttributes { get; set; }
    DbSet<ItemSize> ItemSizes { get; set; }
    DbSet<ItemTax> ItemTaxes { get; set; }
    DbSet<ItemTopping> ItemToppings { get; set; }
    DbSet<OptionAttribute> OptionAttributes { get; set; }
    DbSet<OptionAttributeValue> OptionAttributeValues { get; set; }
    DbSet<Topping> Toppings { get; set; }
}