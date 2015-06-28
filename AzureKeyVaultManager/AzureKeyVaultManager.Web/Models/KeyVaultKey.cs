using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureKeyVaultManager.Web.Models
{
    public class KeyVaultKey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedOn { get; set; }
        public string ExpiresOn { get; set; }
    }
}