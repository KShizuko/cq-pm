using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public unsafe struct MovementPacket
    {
        public ushort Size; //0
        public ushort Type; //2
        public uint UID; //4
        public byte Direction; //8
        public int Running; //9

        public static MovementPacket Create()
        {
            MovementPacket retn = new MovementPacket();
            retn.Size = 12;
            retn.Type = 1005;
            return retn;
        }
    }
}