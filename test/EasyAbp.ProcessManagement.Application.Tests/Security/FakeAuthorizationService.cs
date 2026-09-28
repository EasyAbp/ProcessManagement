using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization;
using Volo.Abp.Security.Claims;

namespace EasyAbp.ProcessManagement.Security;

/// <summary>
/// Grants every policy like ABP's <see cref="AlwaysAllowAuthorizationService"/>, except the ones a test adds to
/// <see cref="DeniedPolicies"/>, so a test can call an app service as a user who lacks a specific permission.
/// </summary>
public class FakeAuthorizationService : IAbpAuthorizationService
{
    public IServiceProvider ServiceProvider { get; }

    public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;

    public HashSet<string> DeniedPolicies { get; } = [];

    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public FakeAuthorizationService(IServiceProvider serviceProvider, ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        ServiceProvider = serviceProvider;
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource,
        IEnumerable<IAuthorizationRequirement> requirements)
    {
        return Task.FromResult(AuthorizationResult.Success());
    }

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
    {
        return Task.FromResult(DeniedPolicies.Contains(policyName)
            ? AuthorizationResult.Failed()
            : AuthorizationResult.Success());
    }
}
