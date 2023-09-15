using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Client;
using ConquerServer_v2.Packet_Structures;
using ConquerServer_v2.Database;
using ConquerServer_v2.Core;

namespace ConquerServer_v2.Packet_Processor
{
    public unsafe partial class PacketProcessor
    {
        public static void CompleteLogin(GameClient Client, DataPacket* Packet)
        {
            if ((Client.ServerFlags & ServerFlags.LoggedIn) != ServerFlags.LoggedIn)
            {
                Client.ServerFlags |= ServerFlags.LoggedIn;
                for (ItemPosition p = Item.FirstSlot; p <= Item.LastSlot; p++)
                {
                    if (Client.Equipment[p] != null)
                    {
                        ServerDatabase.LoadItemStats(Client, Client.Equipment[p]);
                        Client.Equipment[p].Send(Client);
                    }
                }
                Client.CalculateBonus();
                Client.CalculateAttack();
                Client.Stamina = 100;
                if (Client.Entity.Dead)
                    Client.RevivePlayer(false);
                else
                    Client.Entity.Hitpoints = Math.Min(Client.Entity.Hitpoints, Client.Entity.MaxHitpoints);

                Client.Manapoints = Math.Min(Client.Manapoints, Client.MaxManapoints);
                BigUpdatePacket big = new BigUpdatePacket(4);
                big.UID = Client.Entity.UID;
                big.HitpointsAndMana(Client, 0);
                big.Append(2, UpdateID.Stamina, Client.Stamina);
                big.Append(3, UpdateID.RaiseFlag, Client.Entity.StatusFlag);
                Client.Send(big);
                Client.DisplayStats();
            }
        }
    }
}