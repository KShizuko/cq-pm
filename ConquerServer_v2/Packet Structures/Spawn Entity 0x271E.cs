using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ConquerServer_v2.Client;
using ConquerServer_v2.Core;

namespace ConquerServer_v2.Packet_Structures
{
    /// <summary>
    /// 0x271E (Server->Client)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct SpawnEntityPacket
    {
        [FieldOffset(0)]
        public ushort Size;
        [FieldOffset(2)]
        public ushort Type;
        [FieldOffset(4)]
        public uint UID;
        [FieldOffset(8)]
        public uint Model;
        [FieldOffset(12)]
        public ulong StatusFlag;
        [FieldOffset(20)]
        public ushort GuildID;
        [FieldOffset(22)]
        public byte Junk1;
        [FieldOffset(23)]
        public GuildRank GuildRank;
        [FieldOffset(24)]
        public uint GarmentID;
        [FieldOffset(28)]
        public uint HelmetID;
        [FieldOffset(32)]
        public uint ArmorID;
        [FieldOffset(36)]
        public uint LeftHandID;
        [FieldOffset(40)]
        public uint RightHandID;
        [FieldOffset(44)]
        public uint Junk2;
        [FieldOffset(48)]
        public ushort Hitpoints;
        [FieldOffset(50)]
        public ushort Level;
        [FieldOffset(52)]
        public ushort X;
        [FieldOffset(54)]
        public ushort Y;
        [FieldOffset(56)]
        public ushort Hairstyle;
        [FieldOffset(58)]
        public ConquerAngle Facing;
        [FieldOffset(59)]
        public ConquerAction Action;
        [FieldOffset(60)]
        public byte Reborn;
        [FieldOffset(62)]
        public ushort LevelPotency;
        [FieldOffset(64)]
        public byte OtherPotency;
        [FieldOffset(65)]
        public uint Nobility;
        [FieldOffset(66)]
        public ushort Junk3;
        [FieldOffset(68)]
        public uint Junk4;
        [FieldOffset(72)]
        public uint Junk5;
        [FieldOffset(76)]
        public uint Junk6;
        [FieldOffset(80)]
        public byte StringsCount;
        [FieldOffset(81)]
        public byte NameLength;
        [FieldOffset(82)]
        public fixed byte Strings[16];

        public void SetName(string value)
        {
            string m_Name = value;
            if (m_Name.Length > 15)
                m_Name = m_Name.Substring(0, 15);
            Size = (byte)(82 + m_Name.Length);
            StringsCount = 1;
            NameLength = (byte)m_Name.Length;
            fixed (byte* ptr = Strings)
            {
                MSVCRT.memset(ptr, 0, 16);
                value.CopyTo(ptr);
            }
        }

        public static SpawnEntityPacket Create()
        {
            // Size and TQServer are appended when SetName() is called.
            SpawnEntityPacket packet = new SpawnEntityPacket();
            packet.Type = 1014;
            return packet;
        }
    }
}
