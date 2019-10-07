using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Azure;
using Microsoft.Azure.KeyVault;
using MyAddressBook_.EncryptionHelpers;
using MyAddressBook_.Services;

namespace MyAddressBookPlus
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                var keyValut = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(KeyVaultService.GetToken));

                var sec = keyValut.GetSecretAsync(WebConfigurationManager.AppSettings["RedisKeyVaultSecret"]).Result;

                KeyVaultService.CacheConnection = sec.Value;

                AlwaysEncryptedInitializer.InitializeAzureKeyVaultProvider();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
