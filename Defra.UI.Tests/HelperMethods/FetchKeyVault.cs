
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Defra.UI.Tests.Configuration;

namespace Defra.UI.Tests.HelperMethods
{
    public interface IFetchKeyVault
    {
        public string GetKeyVaultValue(string key);
    }
    public class FetchKeyVault : IFetchKeyVault
    {
        private static AzureConnectionConfig AzureConnectionConfig => ConfigSetup.BaseConfiguration.AzureConnectionConfig;
        public string GetKeyVaultValue(string key)
        {
            string KeyVaultUrl = AzureConnectionConfig.KeyVaultUrl;
            string TenandId = AzureConnectionConfig.TenandId;
            string ClientId = AzureConnectionConfig.ClientId;
            string ClientSecret = AzureConnectionConfig.ClientSecret;

            var credential = new ClientSecretCredential(TenandId, ClientId, ClientSecret);

            return string.Empty;
        }

    }
}
