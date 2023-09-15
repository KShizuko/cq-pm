using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    /// <summary>
    /// 0x41F (Server->Client)
    /// </summary>
    public unsafe struct AuthResponsePacket
    {
        public ushort Size;
        public ushort Type;
        public uint Key2;
        public uint Key1;
        private fixed sbyte szIPAddress[16];
        public uint Port;

        public unsafe string IPAddress
        {
            get { fixed (sbyte* bp = szIPAddress) { return new string(bp); } }
            set
            {
                string ip = value;
                fixed (sbyte* bp = szIPAddress)
                {
                    for (int i = 0; i < ip.Length; i++)
                        bp[i] = (sbyte)ip[i];
                }
            }
        }

        public static AuthResponsePacket Create()
        {
            AuthResponsePacket retn = new AuthResponsePacket();
            retn.Size = 0x20;
            retn.Type = 0x41F;
            return retn;
        }
    }
}