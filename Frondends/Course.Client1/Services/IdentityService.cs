using Course.Client1.ConfigurationOptions.Abstract;
using Course.Client1.Models;
using Course.Client1.Services.Abstract;
using Course.Shared.Dtos;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course.Client1.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IClientSettings _clientSettings;
        private readonly IServiceApiSettings _serviceApiSettings;

        public IdentityService(
            HttpClient client,
            IHttpContextAccessor httpContext,
            IClientSettings clientSettings,
            IServiceApiSettings serviceApiSettings)
        {
            _client = client;
            _httpContext = httpContext;
            _clientSettings = clientSettings;
            _serviceApiSettings = serviceApiSettings;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(_serviceApiSettings.BaseUrl);
            if(discovery.IsError)
            {
                throw discovery.Exception;
            }
            var refreshToken = await _httpContext.HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId=_clientSettings.ForUser.ClientId,
                ClientSecret=_clientSettings.ForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint
            };

            var token = await _client.RequestRefreshTokenAsync(refreshTokenRequest);
            if(token.IsError)
            {
                return null;
                //loglama
            }
            AuthenticateResult authenticateResult=await _httpContext.HttpContext.AuthenticateAsync();
            List<AuthenticationToken> authenticationTokens = new List<AuthenticationToken>
            {
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)}
            };
            authenticateResult.Properties.StoreTokens(authenticationTokens);
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticateResult.Principal, authenticateResult.Properties);
            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(_serviceApiSettings.BaseUrl);
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }
            var refreshToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new TokenRevocationRequest()
            {
                ClientId = _clientSettings.ForUser.ClientId,
                ClientSecret = _clientSettings.ForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = OpenIdConnectParameterNames.RefreshToken
            };
            await _client.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SigninInput signinInput)
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(_serviceApiSettings.BaseUrl);
            if (discovery.IsError)
            {
                throw discovery.Exception;
                //loglama
            }

            PasswordTokenRequest passwordTokenRequest = new PasswordTokenRequest()
            {
                Address = discovery.TokenEndpoint,
                ClientId = _clientSettings.ForUser.ClientId,
                ClientSecret = _clientSettings.ForUser.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
            };
            TokenResponse tokenResponse = await _client.RequestPasswordTokenAsync(passwordTokenRequest);
            if (tokenResponse.IsError)
            {
                //loglama
                return Response<bool>.Fail("Kullanıcı Adı veya şifre hatalı", 400);
            }
            var userInfoRequest = new UserInfoRequest()
            {
                Address = discovery.UserInfoEndpoint,
                Token=tokenResponse.AccessToken,
            };
            var userInfoResponse = await _client.GetUserInfoAsync(userInfoRequest);
            if(userInfoResponse.IsError)
            {
                throw userInfoResponse.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims,CookieAuthenticationDefaults.AuthenticationScheme,"name","role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationTokens = new List<AuthenticationToken>
            {
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.AccessToken,Value=tokenResponse.AccessToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.RefreshToken,Value=tokenResponse.RefreshToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };
            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(authenticationTokens);
            authenticationProperties.IsPersistent = signinInput.IsRemember;

            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            return Response<bool>.Success(true,200);
        }
    }
}
