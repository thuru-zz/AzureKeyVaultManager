using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureKeyVaultManager.Web.Models
{
    public class KeyVault
    {
        [DisplayName("Vault URL")]
        [Required]
        [DataType(DataType.Url)]
        public string VaultURl { get; set; }
        
        [DisplayName("Client App ID")]
        [Required]
        [DataType(DataType.Text)]
        public string ClientAppId { get; set; }
        
        [DisplayName("Client App Secret")]
        [Required]
        [DataType(DataType.Text)]
        public string ClientAppSecretKey { get; set; }
    }
}