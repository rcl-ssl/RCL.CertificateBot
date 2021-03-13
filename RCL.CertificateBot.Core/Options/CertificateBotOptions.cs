using System.Collections.Generic;

namespace RCL.CertificateBot.Core
{
    public class CertificateBotOptions
    {
        public string saveCertificatePath { get; set; }
        public string serverIdentifier { get; set; }
        public string[] includeCertificates { get; set; }
        public List<BindingInformation>? bindings { get; set; }
    }
}
