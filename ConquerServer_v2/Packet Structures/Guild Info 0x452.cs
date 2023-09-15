using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    public enum GuildRank : byte
    {
        None = 0x00,
        Member = 0x32,
        InternManager = 0x3C,
        DeputyManager = 0x46,
        BranchManager = 0x50,
        DeputyLeader = 0x5A,
        Leader = 0x64
    }

    public unsafe struct GuildInfoPacket
    {
        public ushort Size; //0
        public ushort Type; //2
        public uint ID; ///4
        public uint Donation; //8
        public uint Fund; //12
        public uint MemberCount; //16
        public GuildRank Rank; //20
        public fixed sbyte Leader[16]; //21
        public fixed sbyte Junk[3]; //37

        public static GuildInfoPacket Create()
        {
            GuildInfoPacket retn = new GuildInfoPacket();
            retn.Size = 0x28;
            retn.Type = 0x452;
            return retn;
        }
    }
}