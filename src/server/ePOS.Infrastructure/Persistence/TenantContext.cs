using System.Reflection;
using ePOS.Application.Common.Contracts;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.FileAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Domain.OrderAggregate;
using ePOS.Domain.TenantAggregate;
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
    public DbSet<Tenant> Tenants { get; set; } = default!;
    public DbSet<TenantTax> TenantTaxes { get; set; } = default!;
    public DbSet<Unit> Units { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<CategoryItem> CategoryItems { get; set; } = default!;
    public DbSet<Item> Items { get; set; } = default!;
    public DbSet<ItemImage> ItemImages { get; set; } = default!;
    public DbSet<ItemSize> ItemSizes { get; set; } = default!;
    public DbSet<ItemTax> ItemTaxes { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderItem> OrderItems { get; set; } = default!;

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
        builder.Entity<CategoryItem>().HasKey(x => new { x.CategoryId, x.ItemId });
        ModelCreating<Item>(builder, entityBuilder => entityBuilder.HasIndex(x => x.Sku));
        ModelCreating<ItemImage>(builder);
        ModelCreating<ItemSize>(builder);
        ModelCreating<ItemTax>(builder);
        ModelCreating<Order>(builder);
        ModelCreating<OrderItem>(builder);
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