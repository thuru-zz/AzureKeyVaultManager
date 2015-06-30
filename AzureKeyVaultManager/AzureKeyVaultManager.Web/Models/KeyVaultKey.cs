using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AzureKeyVaultManager.Web.Models
{
    public class KeyVaultKey
    {
        [DisplayName("Key Name")]
        public string Name { get; set; }
        [DisplayName("Created On")]
        public string CreatedOn { get; set; }
        [DisplayName("Expires On")]
        public string ExpiresOn { get; set; }
    }
}