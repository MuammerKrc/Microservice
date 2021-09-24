// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Course.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resource_catalog"){Scopes=new[]{"catalog_fullpermission"} },
                new ApiResource("resource_order"){Scopes=new[]{ "order_fullpermission" } },
                new ApiResource("resource_payment"){Scopes=new[]{ "payment_fullpermission" } },
                new ApiResource("resource_discount"){Scopes=new[]{"discount_fullpermission"} },
                new ApiResource("resource_basket"){Scopes=new[]{"basket_fullpermission"}},
                new ApiResource("resource_photo_stock"){Scopes=new[]{"photo_stock_fullpermission"}},
                new ApiResource("resource_gateway"){Scopes=new[]{"gateway_fullpermission"}},
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role"} }
            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("catalog_fullpermission","catalog api için bütün izinler"),
                new ApiScope("gateway_fullpermission","catalog api için bütün izinler"),
                new ApiScope("order_fullpermission","catalog api için bütün izinler"),
                new ApiScope("payment_fullpermission","catalog api için bütün izinler"),
                new ApiScope("discount_fullpermission","catalog api için bütün izinler"),
                new ApiScope("basket_fullpermission","catalog api için bütün izinler"),
                new ApiScope("photo_stock_fullpermission","catalog api için bütün izinler"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientName="Client Mvc",
                    ClientId="IdentityClient",
                    ClientSecrets=new[]{new Secret( "secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes=
                    {
                        "catalog_fullpermission",
                        IdentityServerConstants.LocalApi.ScopeName,
                        "photo_stock_fullpermission",
                        "gateway_fullpermission"
                    },
                    AccessTokenLifetime=1*60*60
                },
                new Client()
                {
                    ClientName="Identity Mvc",
                    ClientId="IdentityPassword",
                    ClientSecrets=new[]{new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes=
                    {
                        "catalog_fullpermission",
                        "photo_stock_fullpermission",
                        "basket_fullpermission",
                        "discount_fullpermission",
                        "order_fullpermission",
                        "payment_fullpermission",
                        "gateway_fullpermissin",
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                        "roles"
                    },
                    AllowOfflineAccess=true,
                    AccessTokenLifetime=2*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime=60*60*60
                }
            };
        }

    }
}