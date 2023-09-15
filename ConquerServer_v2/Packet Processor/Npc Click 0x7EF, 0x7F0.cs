using System;
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
        public static void NpcStartup(GameClient Client, NpcClickPacket* Packet)
        {
            Client.NpcLink.ClearSession();
            if (ServerDatabase.NpcDistanceCheck(Packet->NpcID, Client.CurrentDMap, Client.Entity.X, Client.Entity.Y))
            {
                Client.Send(new MessagePacket("Active NpcID: " + Packet->NpcID.ToString(), 0x00FF0000, ChatID.TopLeft));
                Client.ActiveNpcID = Packet->NpcID;
                ExecuteScriptThread.Add(Client, Packet->OptionID, Packet->Input);    
            }
        }

        public static void NpcContinue(GameClient Client, NpcClickPacket* Packet)
        {
            if (Packet->ResponseID == NpcClickID.DeleteGuildMember)
            {
                if (Client.Guild.ID != 0 && Client.Guild.Rank == GuildRank.Leader)
                {
                    ProcessServerCommand(Client, "@kickguild " + Packet->Input, false);
                    Client.Send(Client.Guild.QueryMemberList(0));
                }
            }
            else
            {
                if (ServerDatabase.NpcDistanceCheck(Client.ActiveNpcID, Client.CurrentDMap, Client.Entity.X, Client.Entity.Y))
                {
                    if (Packet->OptionID == 255)
                        return;
                    ExecuteScriptThread.Add(Client, Packet->OptionID, Packet->Input);
                }
            }
        }
    }
}