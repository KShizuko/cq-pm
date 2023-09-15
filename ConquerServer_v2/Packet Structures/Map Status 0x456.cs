using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    public unsafe struct MapStatusPacket
    {
        public ushort Size;
        public ushort Type;
        private uint Region;
        private uint ID;
        public uint Status;

        public uint MapID
        {
            get { return ID; }
            set { Region = ID = value; }
        }

        public static MapStatusPacket Create()
        {
            MapStatusPacket retn = new MapStatusPacket();
            retn.Size = 0x10;
            retn.Type = 0x456;
            return retn;
        }
    }
}
