using System.Collections.Generic;

namespace RCL.CertificateBot.Core
{
    public class HostedSite
    {
        public string siteName { get; set; }
        public List<BindingInformation> bindings { get; set; }
    }
}
