using System;
using System.Security.Cryptography;
using System.Text;

namespace Vendor.Vsecurity
{
    public static class Cryptic
    {
        public static string Encode(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            byte[] inArray = HashAlgorithm.Create("MD5").ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }

        public static bool VerifyMd5Hash(string value, string hash)
        {
            string hashOfInput = Encode(value);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            return false;
        }
    }
}
