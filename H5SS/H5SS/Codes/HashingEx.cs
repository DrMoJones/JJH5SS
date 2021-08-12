using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace H5SS.Codes
{
    public class HashingEx
    {
        public string GetHashed(string valueToHash)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(valueToHash);
            byte[] valueT = MD5.HashData(valueAsBytes);
            string hashedValueAsString = Convert.ToBase64String(valueT);
            return hashedValueAsString;
        }

        public string BcryptHash(string password)
        {
            return BC.HashPassword(password);
        }

        public bool BcryptVerify(string password, string hashedPassword)
        {            
            return BC.Verify(password, hashedPassword);
        }
    }
}
