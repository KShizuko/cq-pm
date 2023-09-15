﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using ConquerServer_v2.Client;
using ConquerServer_v2.Database;
using ConquerServer_v2.Packet_Structures;
using ConquerServer_v2.Core;

namespace ConquerServer_v2.Packet_Processor
{
    public unsafe partial class PacketProcessor
    {
        public static void CreateCharacter(GameClient Client, CreateCharacterPacket* Info)
        {
            if (Info->Job != 10 && Info->Job != 20 &&
                Info->Job != 40 && Info->Job != 100)
            {
                Client.Send(new MessagePacket("Invalid class chosen.", "ALLUSERS", 0x00FFFFFF, ChatID.CharacterCreation));
                return;
            }
            if (Info->Mesh != 1003 && Info->Mesh != 1004 &&
                Info->Mesh != 2001 && Info->Mesh != 2002)
            {
                Client.Send(new MessagePacket("Invalid body size chose.", "ALLUSERS", 0x00FFFFFF, ChatID.CharacterCreation));
                return;
            }
            string CharacterName = Info->CharacterName;
            if (ServerDatabase.ValidCharacterName(CharacterName, false))
            {
                if (!ServerDatabase.CharacterExists(CharacterName))
                {
                    ServerDatabase.CreateAccount(CharacterName, Info->Mesh, Info->Job, Client.Account);
                    Client.Send(new MessagePacket("ANSWER_OK", "ALLUSERS", 0x00FFFFFF, ChatID.CharacterCreation));
                }
                else
                {
                    Client.Send(new MessagePacket("This character name is already in use.", "ALLUSERS", 0x00FFFFFF, ChatID.CharacterCreation));
                }
            }
            else
            {
                Client.Send(new MessagePacket("Invalid character name.", "ALLUSERS", 0x00FFFFFF, ChatID.CharacterCreation));
            }
        }
    }
}
