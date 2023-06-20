using Lab4.Data;
using Lab4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Lab4.CustomIdentity
{
    public class CustomUserManager : UserManager<User>
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        public CustomUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger,
        DbContextOptions<ApplicationDbContext> _options)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
            errors, services, logger)
        {
            options = _options;
        }

        public override async Task<IList<string>> GetRolesAsync(User user)
        {
            using(var context = new ApplicationDbContext(options))
            {
                var roles = await context.Roles.Join(
                    context.Users.Where(
                        u => u.Id == user.Id),
                    r => r.Id,
                    u => u.FK_RoleId,
                    (r, u) => r.Name)
                    .ToListAsync();

                return roles;
            }
        }
    }
}
