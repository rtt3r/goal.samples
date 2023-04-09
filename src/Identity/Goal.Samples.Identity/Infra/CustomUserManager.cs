using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Goal.Samples.Identity.Models;

namespace Goal.Samples.Identity.Infra
{
    public class CustomUserManager : UserManager<User>
    {
        public CustomUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger)
            : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            user.NormalizedName = NormalizeName(user.Name);
            return await base.CreateAsync(user, password);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            user.NormalizedName = NormalizeName(user.Name);
            return await base.CreateAsync(user);
        }

        protected override async Task<IdentityResult> UpdateUserAsync(User user)
        {
            user.NormalizedName = NormalizeName(user.Name);
            return await base.UpdateUserAsync(user);
        }
    }
}
