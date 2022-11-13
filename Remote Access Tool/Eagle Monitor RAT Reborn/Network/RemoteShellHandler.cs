﻿using System;

/* 
|| AUTHOR Arsium ||
|| github : https://github.com/arsium       ||
*/

namespace Eagle_Monitor_RAT_Reborn.Network
{
    internal class RemoteShellHandler : IDisposable
    {
        internal ClientHandler clientHandler { get; set; }
        internal string baseIp { get; set; }

        public void Dispose()
        {
            clientHandler.socket.Close();
            if (clientHandler.socket != null)
            {
                clientHandler.socket.Dispose();
                clientHandler.socket = null;
                clientHandler = null;
            }
        }
    }
}
