using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Security
{
    public interface IEncryptionService
    {
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");
        string CreateHash(byte[] data, string hashAlgorithm = "SHA1");
        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        string CreateSaltKey(int size);
    }
}
