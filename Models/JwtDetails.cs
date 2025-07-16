namespace JWTProcessConsole;

public class JwtDetails
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public int ExpiryInMinutes { get; set; }
        public Dictionary<string, string> claims { get; set; }

    }