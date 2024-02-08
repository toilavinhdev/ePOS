using System.Reflection;
using ePOS.Application.Common.Contracts;
using ePOS.Domain.FileAggregate;
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
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ModelCreating<ApplicationUser>(builder);
        ModelCreating<ApplicationRole>(builder);
        ModelCreating<ApplicationFile>(builder);
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