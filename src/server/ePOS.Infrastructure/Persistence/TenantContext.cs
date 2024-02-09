using System.Reflection;
using ePOS.Application.Common.Contracts;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.FileAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Domain.OptionAttributeAggregate;
using ePOS.Domain.TenantAggregate;
using ePOS.Domain.ToppingAggregate;
using ePOS.Domain.UnitAggregate;
using ePOS.Infrastructure.Identity.Aggregate;
using ePOS.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ePOS.Infrastructure.Persistence;

public class TenantContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
    ApplicationRoleClaim, ApplicationUserToken>, ITenantContext
{
    public TenantContext(DbContextOptions<TenantContext> options) : base(options) { }

    public DbSet<ApplicationFile> Files { get; set; } = default!;
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantTax> TenantTaxes { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryItem> CategoryItems { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemImage> ItemImages { get; set; }
    public DbSet<ItemOptionAttribute> ItemOptionAttributes { get; set; }
    public DbSet<ItemSize> ItemSizes { get; set; }
    public DbSet<ItemTax> ItemTaxes { get; set; }
    public DbSet<ItemTopping> ItemToppings { get; set; }
    public DbSet<OptionAttribute> OptionAttributes { get; set; }
    public DbSet<OptionAttributeValue> OptionAttributeValues { get; set; }
    public DbSet<Topping> Toppings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ModelCreating<ApplicationUser>(builder);
        ModelCreating<ApplicationRole>(builder);
        ModelCreating<ApplicationFile>(builder);
        builder.Entity<Tenant>().HasIndex(x => x.Code);
        ModelCreating<TenantTax>(builder);
        ModelCreating<Unit>(builder);
        ModelCreating<Category>(builder);
        ModelCreating<CategoryItem>(builder);
        ModelCreating<Item>(builder, entityBuilder => entityBuilder.HasIndex(x => x.Sku));
        ModelCreating<ItemImage>(builder);
        ModelCreating<ItemOptionAttribute>(builder);
        ModelCreating<ItemSize>(builder);
        ModelCreating<ItemTax>(builder);
        ModelCreating<ItemTopping>(builder);
        ModelCreating<OptionAttribute>(builder);
        ModelCreating<OptionAttributeValue>(builder);
        ModelCreating<Topping>(builder);
    }
    
    private static void ModelCreating<T>(ModelBuilder builder) where T : class, IEntity
    {
        var sequence = $"Sequence_{typeof(T).Name}";
        builder.HasSequence<int>(sequence);
        builder.Entity<T>().Property(x => x.SubId).HasDefaultValueSql($"NEXT VALUE FOR {sequence}");
        builder.Entity<T>().HasIndex(x => x.SubId);
    }
    
    private static void ModelCreating<T>(ModelBuilder builder, Action<EntityTypeBuilder<T>> callBack) where T : class, IEntity
    {
        ModelCreating<T>(builder);
        callBack.Invoke(builder.Entity<T>());
    }
}