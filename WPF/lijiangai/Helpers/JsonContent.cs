using System.Text;

namespace AIVisualwfpnew.Helpers
{
    public class JsonContent : System.Net.Http.StringContent
    {
        public JsonContent(string content) : base(content)
        {
            ResetHeader();
        }

        public JsonContent(string content, Encoding encoding) : base(content, encoding)
        {
            ResetHeader();
        }

        public JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
        {
            ResetHeader();
        }

        private void ResetHeader()
        {
            if (Headers.Contains("Content-Type"))
            {
                Headers.Remove("Content-Type");
                Headers.Add("Content-Type", "Application/Json");
            }
        }
    }
}
