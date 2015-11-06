using Newtonsoft.Json.Linq;

namespace KatanaTokenAuth.Client
{
    public class TokenResponse
    {
        public string Raw { get; private set; }
        public JObject Json { get; private set; }

        public TokenResponse(string raw)
        {
            Raw = raw;
            Json = JObject.Parse(raw);
        }

        public string AccessToken
        {
            get
            {
                return GetStringOrNull("access_token");
            }
        }

        string GetStringOrNull(string name)
        {
            JToken jtoken;
            if (Json.TryGetValue(name, out jtoken))
                return jtoken.ToString();
            return null;
        }
    }
}
