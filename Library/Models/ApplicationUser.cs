using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Library.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string FullName { get; set; }
  }

  public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
  {
    public AppClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager
        , RoleManager<IdentityRole> roleManager
        , IOptions<IdentityOptions> optionsAccessor)
  : base(userManager, roleManager, optionsAccessor)
    { }

    public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
      var principal = await base.CreateAsync(user);

      if (!string.IsNullOrWhiteSpace(user.FullName))
      {
        ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
        new Claim(ClaimTypes.GivenName, user.FullName)
    });
      }

      return principal;
    }
  }
}