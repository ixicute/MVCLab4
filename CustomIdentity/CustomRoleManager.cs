using Lab4.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab4.CustomIdentity
{
    public class CustomRoleManager : RoleManager<Role>
    {
        public CustomRoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }
    }
}
