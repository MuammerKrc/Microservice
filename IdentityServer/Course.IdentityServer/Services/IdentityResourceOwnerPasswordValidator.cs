using Course.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await userManager.FindByEmailAsync(context.UserName);
            if(user==null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Email veya Şifreniz yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }
            bool checkPs = await userManager.CheckPasswordAsync(user, context.Password);
            if(!checkPs)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Email veya Şifreniz yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }
            context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
