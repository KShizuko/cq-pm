using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ConquerServer_v2.Core;

namespace ConquerServer_v2.Packet_Structures
{
    public enum WarehouseActionID : uint
    {
        Show = 0xA00,
        DepositItem = 0xA01,
        WithdrawItem = 0xA02
    }

    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// 0x44E (Server->Client)
    /// </summary>
    public unsafe struct WarehousePacket
    {
        public ushort Size;
        public ushort Type;
        public uint NpcID;
        public WarehouseActionID Action;
        public int Count;
        public uint ItemUID { get { return (uint)Count; } set { Count = (int)value; } }
        public byte ItemStart;

        public static SafePointer Create(int Count)
        {
            int Size = 16 + (sizeof(WarehouseItem) * Count);
            SafePointer SafePtr = new SafePointer(Size);
            WarehousePacket* ptr = (WarehousePacket*)SafePtr.Addr;
            ptr->Size = (ushort)Size;
            ptr->Type = 0x44E;
            ptr->Count = Count;
            return SafePtr;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 24)]
    /// <summary>
    /// An internal structure of the 0x44E packet (Server->Client)
    /// </summary>
    public unsafe struct WarehouseItem
    {
        [FieldOffset(0)]
        public uint UID;
        [FieldOffset(4)]
        public uint ID;
        [FieldOffset(9)]
        public byte SocketOne;
        [FieldOffset(10)]
        public byte SocketTwo;
        [FieldOffset(11)]
        public ushort wUnknown;
        [FieldOffset(13)]
        public byte Plus;
        [FieldOffset(14)]
        public byte Bless;
        [FieldOffset(15)]
        public byte Enchant;
        [FieldOffset(16)]
        public fixed byte bUnknowns[5];

        public Item ToItem()
        {
            Item retn = new Item();
            retn.ID = ID;
            retn.SocketOne = SocketOne;
            retn.SocketTwo = SocketTwo;
            retn.Plus = Plus;
            retn.Bless = Bless;
            retn.Enchant = Enchant;
            return retn;
        }
        public static WarehouseItem Create(Item Base)
        {
            WarehouseItem retn = new WarehouseItem();
            retn.UID = Base.UID;
            retn.ID = Base.ID;
            retn.SocketOne = Base.SocketOne;
            retn.SocketTwo = Base.SocketTwo;
            retn.Plus = Base.Plus;
            retn.Bless = Base.Bless;
            retn.Enchant = Base.Enchant;
            return retn;
        }
    }
}
