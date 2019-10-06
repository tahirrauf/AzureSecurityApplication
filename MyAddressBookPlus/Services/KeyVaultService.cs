using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace MyAddressBook_.Services
{
    public static class KeyVaultService
    {
        public static string CacheConnection { get; set; }

        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(WebConfigurationManager.AppSettings["ClientId"],
                WebConfigurationManager.AppSettings["ClientSecret"]);

            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if(result == null)
            {
                throw new InvalidOperationException("Failed to get JWT Token");
            }

            return result.AccessToken;
        }
    }
}