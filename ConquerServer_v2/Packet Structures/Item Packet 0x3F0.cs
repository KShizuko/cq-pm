using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Core;
using ConquerServer_v2.Client;
using ConquerScriptLinker;

namespace ConquerServer_v2.Packet_Structures
{
    public enum ItemPosition : ushort
    {
        Inventory = 0,
        Head = 1,
        Necklace = 2,
        Armor = 3,
        Right = 4,
        Left = 5,
        Ring = 6,
        Bottle = 7,
        Boots = 8,
        Garment = 9,
    }

    public enum ItemMode : ushort
    {
        Default = 0x01,
        VendByGold = 0x01,
        Trade = 0x02,
        Update = 0x02,
        VendByCps = 0x03,
        View = 0x04
    }

    /// <summary>
    /// 0x3F0 (Server->Client)
    /// </summary>
    public unsafe struct ItemPacket
    {
        public ushort Size; //0
        public ushort Type; //2
        public uint UID; //4
        public uint ID; //8
        public short Amount; //12
        public short MaxAmount; //14
        public ItemMode Mode; //16
        public ItemPosition Position; //18
        public ushort wUnknown; //20
        public short RebornEffect; //22
        public byte SocketOne; //24
        public byte SocketTwo; //25
        public ushort wUnknown2; //26
        public byte Plus; //28
        public byte Bless; //29
        public byte Enchant; //30

        /// <summary>
        /// Creates a new item instance, pre-intializes with these fields:
        /// Mode = ItemMode.Default,
        /// UID = NextUID,
        /// Color = 3
        /// </summary>
        public static ItemPacket Create()
        {
            ItemPacket packet = new ItemPacket();
            packet.Size = 32;
            packet.Type = 0x3f0;
            packet.Mode = ItemMode.Default;
            return packet;
        }
    }
}
