using CryptoHelper;

namespace AuthAPI.Engines.Cryptography {
    public class CryptoEngine : ICryptoEngine {
        public string Hash(string text)
        {
            return Crypto.HashPassword(text);
        }

        public bool HashCheck(string hash, string text)
        {
            return Crypto.VerifyHashedPassword(hash, text);
        }
    }
}