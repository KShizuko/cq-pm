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
        public static void ComposeItems(GameClient Client, ComposeItemPacket* Packet)
        {
            byte minorslot1;
            byte minorslot2;
            byte gem1;
            byte gem2;

            Item main = Client.Inventory.Search(Packet->MainItem);
            Item minor1 = Client.Inventory.Search(Packet->MinorItem1, out minorslot1);
            Item minor2 = Client.Inventory.Search(Packet->MinorItem2, out minorslot2);
            Item gemf = Client.Inventory.Search(Packet->Gem1, out gem1);
            Item gems = Client.Inventory.Search(Packet->Gem2, out gem2);

            if (main != null && minor1 != null && minor2 != null)
            {
                if (main.Plus < Item.MaxPlus)
                {
                    main.Plus += 1;
                    main.SendInventoryUpdate(Client);
                    Client.Inventory.RemoveBySlot(minorslot1);
                    Client.Inventory.RemoveBySlot(minorslot2);
                    if (gemf != null)
                    {
                        Client.Inventory.RemoveBySlot(gem1);
                        if (gems != null)
                        {
                            Client.Inventory.RemoveBySlot(gem2);
                        }
                    }
                }
            }
        }
    }
}