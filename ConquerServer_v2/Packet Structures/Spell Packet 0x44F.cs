using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Client;
using ConquerScriptLinker;

namespace ConquerServer_v2.Packet_Structures
{
    /// <summary>
    /// 0x401 (Server->Client)
    /// </summary>
    public unsafe struct SpellPacket
    {
        public ushort Size;
        public ushort Type;
        public int Experience;
        public ushort ID;
        public ushort Level;

        public static SpellPacket Create()
        {
            SpellPacket packet = new SpellPacket();
            packet.Size = 0x0C;
            packet.Type = 0x44F;
            return packet;
        }
    }
}
