using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
namespace OidcDemo
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api","My api")
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser{SubjectId = "10000", Username = "jesse", Password = "123123"}
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientUri = "http://localhost:5001",
                    LogoUri = "https://chocolatey.org/content/packageimages/aspnetcore-runtimepackagestore.2.1.5.png",
                    AllowRememberConsent = true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = true,
                    // where to redirect to after login
                    RedirectUris = {"http://localhost:5001/signin-oidc"},
                    // where to redirect to after logout
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:5001/signout-callback-oidc"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                    }
                }
            };
        }
    }
}
