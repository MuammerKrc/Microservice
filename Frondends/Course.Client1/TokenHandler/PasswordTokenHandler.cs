using Course.Client1.Exceptions;
using Course.Client1.Services.Abstract;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Client1.TokenHandler
{
    public class PasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<PasswordTokenHandler> _logger;

        public PasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<PasswordTokenHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TokenResponse tokenResponse = await _identityService.GetAccessTokenByRefreshToken();
                if (tokenResponse != null)
                {
                    if (!tokenResponse.IsError)
                    {
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                        response = await base.SendAsync(request, cancellationToken);

                    }
                }
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //hata fırlat
                throw new UnAuthorizeExeption();
            }
            return response;
        }
    }
}
