using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    public unsafe struct CreateCharacterPacket
    {
        public ushort Size;
        public ushort Type;
        private fixed sbyte szAccount[16];
        private fixed sbyte szCharacterName[16];
        private fixed sbyte szPassword[16];
        public string Account { get { fixed (sbyte* bp = szAccount) { return new string(bp); } } }
        public string CharacterName { get { fixed (sbyte* bp = szCharacterName) { return new string(bp); } } }
        public string Password { get { fixed (sbyte* bp = szPassword) { return new string(bp); } } }
        public ushort Mesh;
        public ushort Job;
        public uint UID;
    }
}
