using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlazmaGames.Core.Utils
{
    public sealed class CryptologyUtilities
    {
        /// <summary>
        /// Create a MD5 hash from a string from <paramref name="input"/>.
        /// </summary>
        public static byte[] CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return hashBytes;
            }
        }
    }
}
