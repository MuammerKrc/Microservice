using Course.Client1.ConfigurationOptions;
using Course.Client1.ConfigurationOptions.Abstract;
using Course.Client1.Services.Abstract;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.Client1.Services
{
    public class ClientCredentialsTokenService : IClientCredentialsTokenService
    {
        private readonly IServiceApiSettings serviceApiSettings;
        private readonly IClientSettings clientSettings;
        private readonly IClientAccessTokenCache clientAccessTokenCache;
        private readonly HttpClient client;

        public ClientCredentialsTokenService(
            IServiceApiSettings serviceApiSettings, 
            IClientSettings clientSettings, 
            IClientAccessTokenCache clientAccessTokenCache,
            HttpClient client)
        {
            this.serviceApiSettings = serviceApiSettings;
            this.clientSettings = clientSettings;
            this.clientAccessTokenCache = clientAccessTokenCache;
            this.client = client;
        }

        public async Task<string> GetToken()
        {
            ClientAccessToken currentToken = await clientAccessTokenCache.GetAsync("WebClient");
            if(currentToken!=null)
            {
                return currentToken.AccessToken;
            }
            var disco =await client.GetDiscoveryDocumentAsync(serviceApiSettings.BaseUrl);
            if(disco.IsError)
            {
                throw disco.Exception;
            }

            ClientCredentialsTokenRequest clientCredentials = new ClientCredentialsTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = clientSettings.WebClient.ClientId,
                ClientSecret = clientSettings.WebClient.ClientSecret
            };
            var token =await client.RequestClientCredentialsTokenAsync(clientCredentials);
            if(token.IsError)
            {
                throw token.Exception;
            }
            await clientAccessTokenCache.SetAsync("WebClient", token.AccessToken,token.ExpiresIn);

            return token.AccessToken;
        }
    }
}
