using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logic.Services
{
    public class EncryptionService
    {
        public string Encrypt(string stringToEncrypt)
        {
            //Hash a password using salt and streching
            byte[] encrypted = KeyDerivation.Pbkdf2(
                password: stringToEncrypt,
                salt: Encoding.UTF8.GetBytes("j78Y#p)/saREN!y3@"),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100,
                numBytesRequested: 64);

            var hash = BitConverter.ToString(encrypted).Replace("-", string.Empty);

            return hash;
        }

        //public string Decrypt(string key, string stringToDecrypt)
        //{

        //}
    }
}
