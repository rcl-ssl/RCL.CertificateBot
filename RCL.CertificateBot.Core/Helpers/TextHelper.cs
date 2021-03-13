namespace RCL.CertificateBot.Core
{
    public static class TextHelper
    {
        public static string[] GetStringArray(string text, char delimiter)
        {
            if (!text.Contains(delimiter))
            {
                return new string[] { text };
            }
            else
            {
                return text.Split(delimiter);
            }
        }
    }
}
