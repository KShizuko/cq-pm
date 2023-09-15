using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Client;

namespace ConquerServer_v2.Packet_Structures
{
    /// <summary>
    /// 0x3EE (Server->Client)
    /// </summary>
    public unsafe class CharacterInfoPacket
    {
        public GameClient Client;
        public CharacterInfoPacket(GameClient _Client)
        {
            Client = _Client;
        }
        public static implicit operator byte[](CharacterInfoPacket info)
        {
            byte name_len = (byte)info.Client.Entity.Name.Length;
            byte[] Buffer = new byte[70 + name_len + info.Client.Spouse.Length];
            fixed (byte* Packet = Buffer)
            {
                *((ushort*)(Packet)) = (ushort)(Buffer.Length);
                *((ushort*)(Packet + 2)) = 0x3EE;
                *((uint*)(Packet + 4)) = info.Client.Entity.UID;
                *((uint*)(Packet + 8)) = info.Client.Entity.Spawn.Model;
                *((ushort*)(Packet + 12)) = info.Client.Entity.Spawn.Hairstyle;
                *((int*)(Packet + 14)) = info.Client.Money;
                *((int*)(Packet + 18)) = info.Client.ConquerPoints;
                *((uint*)(Packet + 22)) = info.Client.Experience;
                *((StatData*)(Packet + 46)) = info.Client.Stats;
                *((ushort*)(Packet + 56)) = (ushort)info.Client.Entity.Hitpoints;
                *((ushort*)(Packet + 58)) = (ushort)info.Client.Manapoints;
                *((ushort*)(Packet + 60)) = info.Client.PKPoints;
                Packet[62] = (byte)info.Client.Entity.Spawn.Level;
                Packet[63] = info.Client.Job;
                Packet[65] = (byte)info.Client.Entity.Spawn.Reborn;
                Packet[67] = 0x02;
                Packet[68] = name_len;
                Packet[69 + name_len] = (byte)info.Client.Spouse.Length;
                info.Client.Entity.Name.CopyTo(Packet + 69);
                info.Client.Spouse.CopyTo(Packet + 70 + name_len);
            }
            return Buffer;
        }
    }
}