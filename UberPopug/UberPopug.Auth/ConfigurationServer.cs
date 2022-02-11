using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace UberPopug.Auth.Models
{

    public static class ConfigurationServer
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
              /*  new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.garndma"
                    }
                }*/
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
               
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
               
                new Client {
                    ClientId = "task_tracker",
                    ClientSecrets = { new Secret("popug_task_tracker".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    RedirectUris = { "https://localhost:44317/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44317/Home/Index" },

                    AllowedScopes = {                    
                        
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        
                       
                    },

                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                },
         
            };
    }
}
