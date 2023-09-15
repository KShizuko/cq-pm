using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    public enum VendMode : ushort
    {
        VendByGold = 0x01,
        VendByConquerPoints = 0x03
    }

    public unsafe struct VendingItemPacket
    {
        public ushort Size; //0
        public ushort Type; //2
        public uint UID; //4
        public uint ShopID; //8
        public int Price; //12
        public uint ItemID; //16
        public short Amount; //20
        public short MaxAmount; //22
        public VendMode Mode; //24
        public fixed byte Junk[4]; // (?) //28
        public short RebornEffect; //32
        public byte SocketOne; //34
        public byte SocketTwo; //35
        public ushort wUnknown; //36
        public byte Plus; //38
        public byte Bless; //39
        public byte Enchant; //40

        public static VendingItemPacket Create()
        {
            VendingItemPacket retn = new VendingItemPacket();
            retn.Size = 42;
            retn.Type = 0x454;
            return retn;
        }
    }
}
