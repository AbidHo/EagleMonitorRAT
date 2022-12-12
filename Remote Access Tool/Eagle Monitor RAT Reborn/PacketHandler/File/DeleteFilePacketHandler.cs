﻿using Eagle_Monitor_RAT_Reborn.Network;
using PacketLib.Packet;
using System.Windows.Forms;

/* 
|| AUTHOR Arsium ||
|| github : https://github.com/arsium       ||
*/

namespace Eagle_Monitor_RAT_Reborn.PacketHandler
{
    internal class DeleteFilePacketHandler
    {
        public DeleteFilePacketHandler(DeleteFilePacket deleteFilePacket) : base()//, ClientHandler clientHandler) : base() 
        {
            try
            {
                if (deleteFilePacket.deleted)
                {
                    ClientHandler.ClientHandlersList[deleteFilePacket.BaseIp].ClientForm.fileManagerDataGridView.BeginInvoke((MethodInvoker)(() =>
                    {
                        try
                        {
                            DataGridViewRow row = ClientHandler.ClientHandlersList[deleteFilePacket.BaseIp].ClientForm.DeleteList[deleteFilePacket.fileTicket];
                            ClientHandler.ClientHandlersList[deleteFilePacket.BaseIp].ClientForm.fileManagerDataGridView.Rows.Remove(row);
                            ClientHandler.ClientHandlersList[deleteFilePacket.BaseIp].ClientForm.DeleteList.Remove(deleteFilePacket.fileTicket);
                        }
                        catch { }
                    }));
                }
            }
            catch { }
        }
    }
}
