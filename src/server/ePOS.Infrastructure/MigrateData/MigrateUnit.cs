using ePOS.Domain.UnitAggregate;
using ePOS.Infrastructure.Persistence;

namespace ePOS.Infrastructure.MigrateData;

public class MigrateUnit
{
    public static async Task SeedUnitsAsync(TenantContext context)
    {
        foreach (var unit in Units.Where(unit => !context.Units.Any(x => x.Name.Equals(unit.Name))))
        {
            await context.AddAsync(unit);
            unit.SetCreationTracking(default, null);
        }
        await context.SaveChangesAsync();
    }

    private static readonly List<Unit> Units = new List<Unit>()
    {
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "inch (in)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "kilogram (kg)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "centimeter (cm)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "cup (c)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "litre (l)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "meter (m)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "milligram (mg)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "millilitre (ml)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "gram (g)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "pieces (pcs)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "tablespoon (tbsp)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
        new ()
        {
            Id = Guid.NewGuid(),
            Name = "teaspoon (tsp)",
            IsDefault = true,
            TenantId = Guid.Empty
        },
    };
}