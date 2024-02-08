using ePOS.Infrastructure.Identity.Aggregate;
using ePOS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ePOS.Infrastructure.MigrateData;

public class MigrateUser
{
    public static async Task SeedUsersAsync(TenantContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        if (!await context.Users.AnyAsync(x => x.Email.Equals("admin@epos.com")))
        {
            var userBaseEntry = await context.Users.AddAsync(new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                FullName = "Admin",
                Email = "adminepos@gmail.com",
                UserName = "adminepos@gmail.com",
                TenantId = Guid.NewGuid(),
                IsAdmin = true,
                Status = UserStatus.Active,
                CreatedAt = DateTimeOffset.Now
            });
            
            await userManager.CreateAsync(userBaseEntry.Entity, "Admin@123");
            await context.SaveChangesAsync();
        }
    }
}