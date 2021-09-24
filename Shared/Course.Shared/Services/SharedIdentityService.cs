using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Course.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
               _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId => _httpContextAccessor.HttpContext.User.Claims.Where(i => i.Type == "sub").SingleOrDefault().Value;
    }
}
