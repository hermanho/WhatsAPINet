
using System.Security.Cryptography;
using System;
using System.Text;
namespace WhatsAppApi
{

    public class KeyStream
    {
        private RC4 rc4;

        private HMACSHA1 mac;

        public KeyStream(byte[] key)
        {
            this.rc4 = new RC4(key, 0x100);
            this.mac = new HMACSHA1(key);
        }

        public void DecodeMessage(byte[] buffer, int macOffset, int offset, int length)
        {
            byte[] numArray = this.mac.ComputeHash(buffer, offset, length);
            int num = 0;
            while (num < 4)
            {
                if (buffer[macOffset + num] == numArray[num])
                {
                    num++;
                }
                else
                {
                    // :MOD: Original exception hasn't been ported here.
                    throw new Exception("Invalid MAC");
                    //throw new FunXMPP.CorruptStreamException("invalid MAC");
                }
            }
            this.rc4.Cipher(buffer, offset, length);
        }

        public void EncodeMessage(byte[] buffer, int macOffset, int offset, int length)
        {
            this.rc4.Cipher(buffer, offset, length);
            byte[] numArray = this.mac.ComputeHash(buffer, offset, length);
            Array.Copy(numArray, 0, buffer, macOffset, 4);
        }

        public static byte[] KeyFromNonce(byte[] nonce, string imei_password)
        {
            // :MOD: Originally this retrieved the UID and apparently hashed it. We have added the new
            // password parameter to the method. imei_password is already hashed.
            string password = imei_password;

            return KeyStream.KeyFromPasswordAndNonce(Encoding.UTF8.GetBytes(password), nonce);
        }

        public static byte[] KeyFromPasswordAndNonce(byte[] pass, byte[] nonce)
        {
            Rfc2898DeriveBytes rfc2898DeriveByte = new Rfc2898DeriveBytes(pass, nonce, 16);
            return rfc2898DeriveByte.GetBytes(20);
        }
    }

}