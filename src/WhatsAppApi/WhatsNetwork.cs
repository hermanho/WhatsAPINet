using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using WhatsAppApi.Helper;

namespace WhatsAppApi
{
    public class WhatsNetwork
    {
        private readonly int recvTimeout;
        private readonly string whatsHost;
        private readonly int whatsPort;

        private List<byte> incomplete_message = new List<byte>();
        private Socket socket;

        public bool SocketStatus
        {
            get { return socket.Connected; }
        }

        public WhatsNetwork(string whatsHost, int port, int timeout = 2000)
        {
            this.recvTimeout = timeout;
            this.whatsHost = whatsHost;
            this.whatsPort = port;
            this.incomplete_message = new List<byte>();
        }

        public void Connect()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.Connect(this.whatsHost, this.whatsPort);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.recvTimeout);

            if (!this.socket.Connected)
                throw new ConnectionException("Cannot connect");
        }

        public void Disconenct()
        {
            if (this.socket.Connected)
                this.socket.Disconnect(true);
        }

        public byte[] ReadData()
        {
            List<byte> buff = new List<byte>();
            byte[] ret = SocketRecv(1024);
            return ret;
        }

        public void SendData(string data)
        {
            SocketSend(data, data.Length, 0);
        }
        public void SendData(byte[] data)
        {
            SocketSend(data);
        }

        /// <summary>
        /// Receives the specified number of bytes.
        /// </summary>
        /// <param name="length">Number of bytes to receive at most.</param>
        /// <returns>A byte array containing bytes read.</returns>
        /// <remarks>If the connection is closed by the remote end, or if any error occurs, 
        /// a ConnectionException is thrown.</remarks>
        private byte[] SocketRecv(int length)
        {
            if (!socket.Connected)
            {
                string msg = "WhatsNetwork exception: Socket is not connected";
                Console.Error.WriteLine(msg);
                throw new ConnectionException(msg);
            }

            var buff = new byte[length];
            int receiveLength = 0;
            do
            {
                try
                {
                    receiveLength = socket.Receive(buff, 0, length, 0);

                    // If it returns zero, the other end has closed
                    // the connection.
                    if (receiveLength == 0)
                    {
                        socket.Close();
                        string msg = "WhatsNetwork exception: Remote end closed the connection";
                        Console.Error.WriteLine(msg);
                    }
                }
                catch (SocketException excpt)
                {
                    if (excpt.SocketErrorCode == SocketError.TimedOut)
                    {
                        string msg = "WhatsNetwork: Receive operation timed out";
                        Console.Error.WriteLine(msg);
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("WhatsNetwork: Receive operation failed: {0}", excpt);
                        throw new ConnectionException("WhatsNetwork: Receive operation failed.", excpt);
                    }
                }
            } while (receiveLength <= 0);

            byte[] tmpRet = new byte[receiveLength];
            if (receiveLength > 0)
                Buffer.BlockCopy(buff, 0, tmpRet, 0, receiveLength);

            return tmpRet;
        }

        private void SocketSend(string data, int length, int flags)
        {
            var tmpBytes = WhatsApp.SYSEncoding.GetBytes(data);
            this.socket.Send(tmpBytes);
        }

        private void SocketSend(byte[] data)
        {
            this.socket.Send(data);
        }

    } //! class
} //! namespace
