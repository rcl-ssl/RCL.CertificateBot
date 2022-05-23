#nullable disable

namespace RCL.CertificateBot.Core
{
    public class IISBindingInformation
    {
        public string siteName { get; set; }
        public string ip { get; set; }
        public string port { get; set; }
        public string host { get; set; }
        public string certificateName { get; set; }

        public string GetBindingInformation()
        {
            return $"{ip}:{port}:{host}";
        }
    }
}