#nullable disable

namespace RCL.CertificateBot.Core
{
    public class CertificateBotOptions
    {
        public string SaveCertificatePath { get; set; }
        public List<string> IncludeCertificates { get; set; }
        public List<IISBindingInformation> IISBindings { get; set; }
    }
}
