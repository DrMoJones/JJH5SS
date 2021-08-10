using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace H5SS.Codes
{
    public class Class
    {
        public string GetHashed(string valueToHash)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(valueToHash);
            byte[] valueT = MD5.HashData(valueAsBytes);
            string hashedValueAsString = Convert.ToBase64String(valueT);
            return hashedValueAsString;
        }
    }
}
