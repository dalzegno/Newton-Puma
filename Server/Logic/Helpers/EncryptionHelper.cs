using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Logic.Services
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string stringToEncrypt)
        {
            //Hash a password using salt and streching
            byte[] encrypted = KeyDerivation.Pbkdf2(
                password: stringToEncrypt,
                salt: Encoding.UTF8.GetBytes("j78Y#p)/saREN!y3@"),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100,
                numBytesRequested: 64);

            return BitConverter.ToString(encrypted).Replace("-", string.Empty);
        }
    }
}
