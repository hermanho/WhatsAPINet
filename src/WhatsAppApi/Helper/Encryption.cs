using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WhatsAppApi.Helper
{

    // TODO: As of now, different Encryption instances seem to be created for writing and reading.
    // Thus, one of the two is currently unused. Should eventually be refactored.

    public class Encryption
    {
        public RC4 encryptionOutgoing = null;
        public RC4 encryptionIncoming = null;

        // TODO: This probably shouldnt be accessible, but as of now it is useful for it to be accessible
        // because it is used to get some hashes.
        public byte[] Key { get; set; }

        public Encryption(byte [] key)
        {
            encryptionOutgoing = new RC4(key, 256);
            encryptionIncoming = new RC4(key, 256);
            Key = key;
        }

        public byte[] WhatsappEncrypt(byte[] data, bool appendHash)
        {
            HMACSHA1 h = new HMACSHA1(Key);
            byte[] buff = new byte[data.Length];
            Buffer.BlockCopy(data, 0, buff, 0, data.Length);

            encryptionOutgoing.Cipher(buff);
            byte[] hashByte = h.ComputeHash(buff);
            byte[] response = new byte[4 + buff.Length];
            if (appendHash)
            {
                Buffer.BlockCopy(buff, 0, response, 0, buff.Length);
                Buffer.BlockCopy(hashByte, 0, response, buff.Length, 4);
            }
            else
            {
                Buffer.BlockCopy(hashByte, 0, response, 0, 4);
                Buffer.BlockCopy(buff, 0, response, 4, buff.Length);
            }

            return response;
        }

        public byte[] WhatsappDecrypt(byte[] data)
        {
            if (encryptionIncoming == null)
                encryptionIncoming = new RC4(Key, 256);
            byte[] buff = new byte[data.Length];
            Buffer.BlockCopy(data, 0, buff, 0, data.Length);
            encryptionIncoming.Cipher(buff);
            return buff;
        }
    }
}
