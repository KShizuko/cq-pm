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
        public uint Model;
        [FieldOffset(8)]
        public uint UID;
        [FieldOffset(12)]
        public ushort GuildID;
        [FieldOffset(15)]
        public GuildRank GuildRank;
        [FieldOffset(16)]
        public ulong StatusFlag;

        [FieldOffset(24)]
        public uint HelmetID;
        [FieldOffset(28)]
        public uint HorseID;
        [FieldOffset(32)]
        public uint ArmorID;
        [FieldOffset(36)]
        public uint LeftHandID;
        [FieldOffset(40)]
        public uint RightHandID;

        [FieldOffset(48)]
        public ushort Hitpoints;
        [FieldOffset(50)]
        public ushort Level;
        [FieldOffset(52)]
        public ushort Hairstyle;
        [FieldOffset(54)]
        public ushort X;
        [FieldOffset(56)]
        public ushort Y;
        [FieldOffset(58)]
        public ConquerAngle Facing;
        [FieldOffset(59)]
        public ConquerAction Action;
        [FieldOffset(64)]
        public byte Reborn;

        [FieldOffset(65)]
        public ushort LevelPotency;
        [FieldOffset(70)]
        public int OtherPotency;
        [FieldOffset(84)]
        public NobilityID Nobility;

        [FieldOffset(88)]
        public ushort ArmorColor;
        [FieldOffset(90)]
        public ushort ShieldColor;
        [FieldOffset(92)]
        public ushort HeadColor;

        [FieldOffset(110)]
        public byte StringsCount;
        [FieldOffset(111)]
        public byte NameLength;
        [FieldOffset(112)]
        public fixed byte Strings[24];

        public void SetName(string value)
        {
            string m_Name = value;
            if (m_Name.Length > 15)
                m_Name = m_Name.Substring(0, 15);
            Size = (byte)(0x70 + m_Name.Length);
            StringsCount = 1;
            NameLength = (byte)m_Name.Length;
            fixed (byte* ptr = Strings)
            {
                MSVCRT.memset(ptr, 0, 24);
                value.CopyTo(ptr);
                PacketBuilder.AppendTQServer(ptr + NameLength, 8);
            }
        }

        public static SpawnEntityPacket Create()
        {
            // Size and TQServer are appended when SetName() is called.
            SpawnEntityPacket packet = new SpawnEntityPacket();
            packet.Type = 0x271E;
            return packet;
        }
    }
}
