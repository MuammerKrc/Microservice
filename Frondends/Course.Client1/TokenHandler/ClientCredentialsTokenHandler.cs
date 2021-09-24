using Course.Client1.Exceptions;
using Course.Client1.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Client1.TokenHandler
{
    public class ClientCredentialsTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialsTokenService clientCredentialsTokenService;

        public ClientCredentialsTokenHandler(IClientCredentialsTokenService clientCredentialsTokenService)
        {
            this.clientCredentialsTokenService = clientCredentialsTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await clientCredentialsTokenService.GetToken());

            var response = await base.SendAsync(request, cancellationToken);
            if(response.StatusCode==System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizeExeption();
            }
            return response;
        }
    }
}
