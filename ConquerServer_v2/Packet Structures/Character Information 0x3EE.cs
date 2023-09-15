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
            byte[] Buffer = new byte[90 + 8 + name_len + info.Client.Spouse.Length];
            fixed (byte* Packet = Buffer)
            {
                *((ushort*)(Packet)) = (ushort)(Buffer.Length - 8);
                *((ushort*)(Packet + 2)) = 0x3EE;
                *((uint*)(Packet + 4)) = info.Client.Entity.UID;
                *((uint*)(Packet + 8)) = info.Client.Entity.Spawn.Model;
                *((ushort*)(Packet + 12)) = info.Client.Entity.Spawn.Hairstyle;
                *((int*)(Packet + 14)) = info.Client.Money;
                *((int*)(Packet + 18)) = info.Client.ConquerPoints;
                *((uint*)(Packet + 22)) = info.Client.Experience;

                *((StatData*)(Packet + 50)) = info.Client.Stats;
                *((ushort*)(Packet + 60)) = (ushort)info.Client.Entity.Hitpoints;
                *((ushort*)(Packet + 62)) = (ushort)info.Client.Manapoints;
                *((ushort*)(Packet + 64)) = info.Client.PKPoints;
                Packet[66] = (byte)info.Client.Entity.Spawn.Level;
                Packet[67] = info.Client.Job;
                Packet[69] = (byte)info.Client.Entity.Spawn.Reborn;
                // 71 = quiz show
                /* chunk of data added here for 5130 */

                Packet[87] = 0x02;
                Packet[88] = name_len;
                Packet[89 + name_len] = (byte)info.Client.Spouse.Length;
                info.Client.Entity.Name.CopyTo(Packet + 89);
                info.Client.Spouse.CopyTo(Packet + 90 + name_len);
                PacketBuilder.AppendTQServer(Packet, Buffer.Length);
            }
            return Buffer;
        }
    }
}