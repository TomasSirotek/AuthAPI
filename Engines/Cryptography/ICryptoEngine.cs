namespace AuthAPI.Engines.Cryptography {
    public interface ICryptoEngine {
        /// <summary>
        /// Use Hash to hash a text. See <see href="https://github.com/henkmollema/CryptoHelper">CryptoHelper</see>
        /// </summary>
        string Hash(string text);

        /// <summary>
        /// Use HashCheck to validate the hash of a text. See <see href="https://github.com/henkmollema/CryptoHelper">CryptoHelper</see>
        /// </summary>
        bool HashCheck(string hash, string text);
    }
}