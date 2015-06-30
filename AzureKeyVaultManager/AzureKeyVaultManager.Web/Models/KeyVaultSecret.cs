using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureKeyVaultManager.Web.Models
{
    public class KeyVaultSecret
    {
        public string SecretId { get; set; }

        [Required]
        [DisplayName("Secret Name")]
        public string Name { get; set; }

        [DisplayName("Created On")]
        public string CreatedOn { get; set; }

        [DisplayName("Expires On")]
        public string ExpiresOn { get; set; }
        public string Version { get; set; }

        [Required]
        [DisplayName("Secret Value")]
        public string SecretValue { get; set; }
    }
}