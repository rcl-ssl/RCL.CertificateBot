using RCL.SDK;
using System.Collections.Generic;

namespace RCL.CertificateBot.Core
{
    public class MessageResponse
    {
        public string message { get; set; }
        public List<CertificateResponse>? certificateResponses { get; set; }
    }
}
