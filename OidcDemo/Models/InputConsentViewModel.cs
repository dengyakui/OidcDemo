using System.Collections.Generic;

namespace OidcDemo.Models
{
    public class InputConsentViewModel
    {
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool RememberConsent { get; set; }
        public string Button { get; set; }
        public string ReturnUrl { get; set; }
    }
}