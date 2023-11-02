using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using UnityEngine;

namespace ChaitaesWeb
{
    public class Encryption
    {
        public static string CreateSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
        public static string ByteArrayToHexString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        public static string GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sha256hashstring = new SHA256Managed();
            byte[] hash = sha256hashstring.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}