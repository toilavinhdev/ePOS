namespace ePOS.Application.Common.Contracts;

public interface ITenantContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ());
}