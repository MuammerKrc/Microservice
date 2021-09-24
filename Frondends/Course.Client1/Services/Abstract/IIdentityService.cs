using Course.Client1.Models;
using Course.Shared.Dtos;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Services.Abstract
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signinInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();

    }
}
