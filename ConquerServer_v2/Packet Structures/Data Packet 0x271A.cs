using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerServer_v2.Packet_Structures
{
    public enum DataID : ushort
    {
        None = 0,
        SetLocation = 74,
        Hotkeys = 75,
        ConfirmAssociates = 76,
        ConfirmProficiencies = 77,
        ConfirmSpells = 78,
        ChangeDirection = 79,
        ChangeAction = 81,
        EnterPortal = 85,
        Teleport = 86,
        LevelUp = 92,
        EndXpList = 93,
        Revive = 94,
        DeleteCharacter = 95,
        ChangePkMode = 96,
        ConfirmGuild = 97,
        Mining = 99,
        RequestEntity = 102,
        SetMapColor = 104,
        RequestTeamPosition = 106,
        CorrectCoords = 108,
        UnlearnSpell = 109,
        UnlearnProficiency = 110,
        StartVend = 111,
        GetSurroundings = 114,
        Switch = 116,
        EndTransform = 118,
        EndFly = 120,
        PickupCashEffect = 121,
        GUIDialog = 126,
        GuardJump = 129,
        Login = 130,
        SpawnEffect = 131,
        RemoveEntity = 132,
        Jump = 133,
        ChangeAvatar = 142,
        RequestFriendInfo = 150
    }

    public class DataSwitchArg
    {
        public const uint
            MarriageMouse = 1067,
            EnchantWindow = 1091;
    }

    public class DataGUIDialog
    {
        public const uint
            Warehouse = 4,
            Composition = 1;
    }

    /// <summary>
    /// 0x271A (Server->Client, Client->Server)
    /// </summary>
    public unsafe struct DataPacket
    {
        public ushort Size; //0
        public ushort Type; //2
        public TIME TimeStamp; //4
        public uint UID; //8
        public uint dwParam1; //12
        public ushort dwParam_Lo
        {
            get { return (ushort)dwParam1; }
            set { dwParam1 = (uint)((dwParam_Hi << 16) | value); }
        }
        public ushort dwParam_Hi
        {
            get { return (ushort)(dwParam1 >> 16); }
            set { dwParam1 = (uint)((value << 16) | dwParam_Lo); }
        }
        public ushort wParam1; //16
        public ushort wParam2; //18
        public ushort wParam3; //20
        public DataID ID; //22

        public static DataPacket Create()
        {
            DataPacket packet = new DataPacket();
            packet.Size = 24;
            packet.Type = 1010;
            packet.TimeStamp = TIME.Now;
            return packet;
        }
    }
}
