using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace WhatsUnitTest
{
    class Util
    {
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            StringReader sr = new StringReader(hex);
            for (int i = 0; i < NumberChars; i++)
                bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            sr.Dispose();
            return bytes;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }


        [Test]
        public void StringToByteArrayTest()
        {
            var str = "DEADC0FFEE1337";
            var expected = new byte[] { 0xDE, 0xAD, 0xC0, 0xFF, 0xEE, 0x13, 0x37 };
            var actual = StringToByteArray(str);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToString()
        {
            var expected = "DEADC0FFEE1337";
            var arr = new byte[] { 0xDE, 0xAD, 0xC0, 0xFF, 0xEE, 0x13, 0x37 };
            var actual = ByteArrayToString(arr);

            Assert.AreEqual(expected, actual);
        }
    }
}
