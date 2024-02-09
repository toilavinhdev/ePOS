using ePOS.Domain.TenantAggregate;
using ePOS.Infrastructure.Identity.Aggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ePOS.Infrastructure.Persistence.MigrateData;

public class MigrateUser
{
    public static async Task SeedUsersAsync(TenantContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        if (!await context.Users.AnyAsync(x => x.Email.Equals("adminepos@gmail.com")))
        {
            var tenant = new Tenant()
            {
                Id = Guid.NewGuid(),
                Name = "adminepos",
                Code = Guid.NewGuid().ToString("N"),
                CreatedAt = DateTimeOffset.Now,
                TenantTax = new TenantTax
                {
                    Id = Guid.NewGuid(),
                    IsApplyForAllItem = false,
                    IsPriceIncludeTax = false,
                    Value = 0,
                    CreatedAt = DateTimeOffset.Now
                }
            };
            await context.Tenants.AddAsync(tenant);
            
            var userBaseEntry = await context.Users.AddAsync(new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                FullName = "Admin",
                Email = "adminepos@gmail.com",
                UserName = "adminepos@gmail.com",
                TenantId = tenant.Id,
                IsAdmin = true,
                Status = UserStatus.Active,
                CreatedAt = DateTimeOffset.Now
            });

            tenant.TenantTax.CreatedBy = userBaseEntry.Entity.Id;
            
            await userManager.CreateAsync(userBaseEntry.Entity, "Admin@123");
            await context.SaveChangesAsync();
        }
    }
}