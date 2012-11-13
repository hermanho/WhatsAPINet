using System;

//
// From the WP WhatsApp source.
//
//

namespace WhatsAppApi
{
    public class RC4
    {
        private int[] s;

        private int i;

        private int j;

        /// <summary>
        /// Constructs the RC4 object.
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="drop">Number of bytes to encrypt to initialize the cypher</param>
        public RC4(byte[] key, int drop)
        {
            this.s = new int[0x100];
            this.i = 0;
            while (this.i < (int)this.s.Length)
            {
                this.s[this.i] = this.i;
                RC4 rC4 = this;
                rC4.i = rC4.i + 1;
            }
            this.j = 0;
            this.i = 0;
            while (this.i < (int)this.s.Length)
            {
                this.j = (byte)(this.j + this.s[this.i] + key[this.i % (int)key.Length]);
                RC4.Swap<int>(this.s, this.i, this.j);
                RC4 rC41 = this;
                rC41.i = rC41.i + 1;
            }
            bool flag = false;

            // :MOD: The following lines have been modified because otherwise
            // a bool can't be converted to int.
            int num = (flag ? 1 : 0);
            this.j = (flag ? 1 : 0);

            this.i = num;
            this.Cipher(new byte[drop]);
        }

        public void Cipher(byte[] data, int offset, int length)
        {
            while (true)
            {
                int num = length;
                length = num - 1;
                if (num == 0)
                {
                    break;
                }
                this.i = (this.i + 1) % 0x100;
                this.j = (this.j + this.s[this.i]) % 0x100;
                int num1 = this.s[this.i];
                this.s[this.i] = this.s[this.j];
                this.s[this.j] = num1;
                int num2 = offset;
                offset = num2 + 1;
                data[num2] = (byte)(data[num2] ^ (byte)this.s[(this.s[this.i] + this.s[this.j]) % 0x100]);
            }
        }

        public void Cipher(byte[] data)
        {
            this.Cipher(data, 0, (int)data.Length);
        }

        private static void Swap<T>(T[] a, int i, int j)
        {
            T t = a[i];
            a[i] = a[j];
            a[j] = t;
        }
    }
}