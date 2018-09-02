using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Planner.Helpers
{
    public class EncryptionHelper
    {
        public static string Encrypt(string _password)
        {
            MD5CryptoServiceProvider _sp = new MD5CryptoServiceProvider();
            byte[] _byteValue = System.Text.Encoding.UTF8.GetBytes(_password);
            byte[] _encodedbyte = _sp.ComputeHash(_byteValue);
            return Convert.ToBase64String(_encodedbyte);
        }
    }
}