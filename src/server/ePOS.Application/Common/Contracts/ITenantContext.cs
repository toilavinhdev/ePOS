using ePOS.Domain.FileAggregate;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Common.Contracts;

public interface ITenantContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ());
    DbSet<ApplicationFile> Files { get; set; }
}