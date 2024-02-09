using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ePOS.Application.Commands;
using ePOS.Application.Commands.User;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Queries.User;
using ePOS.Infrastructure.Identity.Aggregate;
using ePOS.Infrastructure.Persistence;
using ePOS.Shared.Constants;
using ePOS.Shared.Exceptions;
using ePOS.Shared.Extensions;
using ePOS.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ePOS.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TenantContext _context;
    private readonly JwtTokenConfig _jwtTokenConfig;

    public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, 
        TenantContext context, AppSettings appSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _context = context;
        _jwtTokenConfig = appSettings.JwtTokenConfig;
    }

    public UserClaimsValue GetUserClaimsValue()
    {
        var authorizationValue = _httpContextAccessor.HttpContext?.Request.Headers
            .FirstOrDefault(x => x.Key.Equals("Authorization")).Value;
        
        var accessToken = authorizationValue?.ToString().Split(" ").LastOrDefault();
        
        if (string.IsNullOrEmpty(accessToken)) return new UserClaimsValue();
        
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        
        var decodedToken = jwtSecurityTokenHandler.ReadJwtToken(accessToken);
        
        return new UserClaimsValue()
        {
            Id = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value.ToGuid(),
            TenantId = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("tenantId"))?.Value.ToGuid() ?? default,
            FullName = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("fullName"))?.Value,
            Email = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value,
        };
    }

    public async Task<SignInResponse> SignInAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email.Equals(command.Email), cancellationToken);
        if (user is null) throw new BadRequestException("EmailNotFound");
        var checkedPassword = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!checkedPassword) throw new BadRequestException("IncorrectPassword");
        
        var userToken = await _context.UserTokens
            .FirstOrDefaultAsync(x => x.UserId.Equals(user.Id) && x.LoginProvider.Equals(LoginProvider.Jwt), cancellationToken);
        
        if (userToken is not null) _context.UserTokens.Remove(userToken);

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        
        await _context.UserTokens.AddAsync(new ApplicationUserToken
        {
            UserId = user.Id,
            LoginProvider = LoginProvider.Jwt,
            Name = user.Email,
            Value = accessToken,
            Expires = DateTime.Now.AddMinutes(_jwtTokenConfig.RefreshTokenExpirationMinutes)
        }, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return new SignInResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpCommand command, Guid tenantId, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            FullName = command.FullName,
            Email = command.Email,
            IsAdmin = true,
            UserName = command.Email,
            Status = UserStatus.Active,
            CreatedAt = DateTimeOffset.Now
        };

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded) throw new BadRequestException(string.Join(";", result.Errors.Select(x => x.Code)));

        await _context.SaveChangesAsync(cancellationToken);
        
        return new SignUpResponse
        {
            Id = Guid.NewGuid(),
            FullName = user.FullName,
            Email = user.Email
        };
    }

    public async Task<GetMeResponse> GetMeAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        if (user is null) throw new RecordNotFoundException(nameof(ApplicationUser));
        return new GetMeResponse()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl
        };
    }
    
    public async Task ChangePasswordAsync(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(command.Email), cancellationToken);
        if (user is null) throw new RecordNotFoundException(nameof(ApplicationUser));
        
        user.ModifiedAt = DateTimeOffset.Now;
        
        var result = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
        if (!result.Succeeded) throw new BadRequestException(string.Join(";", result.Errors.Select(x => x.Code)));
    }
    
    private string GenerateJwtToken(ApplicationUser user) 
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfig.ServerSecretKey)),
            SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new ("id", user.Id.ToString()),  
            new ("tenantId", user.TenantId.ToString()),  
            new ("fullName", $"{user.FullName}"),
            new ("email", user.Email)
        };
        var expires = DateTime.Now.AddMinutes(_jwtTokenConfig.AccessTokenExpirationMinutes);
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials); 
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}