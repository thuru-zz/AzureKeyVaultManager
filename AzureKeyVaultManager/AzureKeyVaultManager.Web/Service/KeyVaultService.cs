using AzureKeyVaultManager.Web.Models;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureKeyVaultManager.Web.Service
{
    public class KeyVaultService
    {
        private KeyVaultClient _keyVaultClient;
        private string _vaultUrl;
        private string _appId;
        private string _appSecret;

        public KeyVaultService(string vaultUrl, string appId, string appSecret)
        {
            _vaultUrl = vaultUrl;
            _appId = appId;
            _appSecret = appSecret;
            _keyVaultClient = new KeyVaultClient(GetAccessTokenAsync);
        }

        #region Secrets

        public List<KeyVaultSecret> GetSecrets()
        {
            var response = _keyVaultClient.GetSecretsAsync(_vaultUrl).GetAwaiter().GetResult();
            
            var result = new List<KeyVaultSecret>();

            foreach (var secret in response.Value)
            {
                result.Add(new KeyVaultSecret()
                {
                    SecretId = secret.Id,
                    Name = secret.Identifier.Name,
                    CreatedOn = secret.Attributes.Created == null ? String.Empty : secret.Attributes.Created.Value.ToShortDateString(),
                    ExpiresOn = secret.Attributes.Expires == null ? String.Empty : secret.Attributes.Expires.Value.ToShortDateString()
                });
            }

            while (!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = _keyVaultClient.GetSecretsNextAsync(response.NextLink).GetAwaiter().GetResult();

                foreach (var secret in response.Value)
                {
                    result.Add(new KeyVaultSecret()
                    {
                        SecretId = secret.Id,
                        Name = secret.Identifier.Name,
                        CreatedOn = secret.Attributes.Created == null ? String.Empty : secret.Attributes.Created.Value.ToShortDateString(),
                        ExpiresOn = secret.Attributes.Expires == null ? String.Empty : secret.Attributes.Expires.Value.ToShortDateString()
                    });
                }
            }

            return result.OrderByDescending(s => s.CreatedOn).ToList();
        }

   
        public List<KeyVaultSecret> GetVersionsOfSecret(string name)
        {
            var response = _keyVaultClient.GetSecretVersionsAsync(_vaultUrl, name).GetAwaiter().GetResult();

            var result = new List<KeyVaultSecret>();
          
            foreach (var secret in response.Value)
            {
                result.Add(new KeyVaultSecret()
                {
                    SecretId = secret.Id,
                    Name = secret.Identifier.Name,
                    Version = secret.Identifier.Version,
                    CreatedOn = secret.Attributes.Created == null ? String.Empty : secret.Attributes.Created.Value.ToShortDateString(),
                    ExpiresOn = secret.Attributes.Expires == null ? String.Empty : secret.Attributes.Expires.Value.ToShortDateString()
                });
            }


            while(!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = _keyVaultClient.GetSecretVersionsNextAsync(response.NextLink).GetAwaiter().GetResult();

                foreach (var secret in response.Value)
                {
                    result.Add(new KeyVaultSecret()
                    {
                        SecretId = secret.Id,
                        Name = secret.Identifier.Name,
                        Version = secret.Identifier.Version,
                        CreatedOn = secret.Attributes.Created == null ? String.Empty : secret.Attributes.Created.Value.ToShortDateString(),
                        ExpiresOn = secret.Attributes.Expires == null ? String.Empty : secret.Attributes.Expires.Value.ToShortDateString()
                    });
                }
            }

            return result;
        }


        public KeyVaultSecret CreateSecret(string name, string value)
        {
            var secret = _keyVaultClient.SetSecretAsync(_vaultUrl, name, value).GetAwaiter().GetResult();

            return new KeyVaultSecret()
            {
                Name = secret.SecretIdentifier.Name,
                Version = secret.SecretIdentifier.Version
            };
        }


        public void DeleteSecret(string name)
        {
            var secret = _keyVaultClient.DeleteSecretAsync(_vaultUrl, name).GetAwaiter().GetResult();
        }

        #endregion


        #region Keys

        public List<KeyVaultKey> GetKeys()
        {
            var response = _keyVaultClient.GetKeysAsync(_vaultUrl).GetAwaiter().GetResult();
            var result = new List<KeyVaultKey>();

            foreach(var key in response.Value)
            {
                result.Add(new KeyVaultKey()
                {
                    Name = key.Identifier.Name,
                    CreatedOn = key.Attributes.Created == null ? String.Empty : key.Attributes.Created.Value.ToShortDateString(),
                    ExpiresOn = key.Attributes.Expires == null ? String.Empty : key.Attributes.Expires.Value.ToShortDateString()
                });
            }

            while(!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = _keyVaultClient.GetKeysNextAsync(response.NextLink).GetAwaiter().GetResult();

                foreach (var key in response.Value)
                {
                    result.Add(new KeyVaultKey()
                    {
                        Name = key.Identifier.Name,
                        CreatedOn = key.Attributes.Created == null ? String.Empty : key.Attributes.Created.Value.ToShortDateString(),
                        ExpiresOn = key.Attributes.Expires == null ? String.Empty : key.Attributes.Expires.Value.ToShortDateString()
                    });
                }
            }

            return result;
        }

        #endregion


        private async Task<string> GetAccessTokenAsync(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential(_appId, _appSecret);

            var authenticationContext = new AuthenticationContext(authority, false);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredential);

            return result.AccessToken;
        }
    }
}