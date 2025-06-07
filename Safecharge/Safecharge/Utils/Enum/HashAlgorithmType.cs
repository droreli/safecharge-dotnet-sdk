namespace Safecharge.Utils.Enum
{
    /// <summary>
    /// This is the hash algorithm which is used to calculate the checksum. It's configured in the server per merchant site.
    /// </summary>
    public enum HashAlgorithmType
    {
        /// <summary>
        /// MD5 Hashing Algorithm.
        /// </summary>
        MD5,
        /// <summary>
        /// SHA-256 Hashing Algorithm. This is the recommended algorithm.
        /// </summary>
        SHA256
    }
}
