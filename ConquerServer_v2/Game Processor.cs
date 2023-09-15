using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Client;
using ConquerServer_v2.Packet_Processor;
using ConquerServer_v2.Packet_Structures;
using ConquerServer_v2.Database;
using ConquerServer_v2.Core;
using ConquerServer_v2.Monster_AI;

namespace ConquerServer_v2
{
    public partial class Program
    {
        static void Game_Connect(NetworkClient nClient)
        {
            GameClient Client = new GameClient(nClient);
            nClient.Owner = Client;
        }

        public unsafe static void Game_Disconnect(NetworkClient nClient)
        {
            if (nClient.Owner != null)
            {
                GameClient Client = nClient.Owner as GameClient;
                nClient.Owner = null;

                if ((Client.ServerFlags & ServerFlags.LoggedOut) != ServerFlags.LoggedOut)
                {
                    Client.ServerFlags |= ServerFlags.LoggedOut;
                    Client.ServerFlags &= ~ServerFlags.LoggedIn;

                    DataPacket unspawn = DataPacket.Create();
                    unspawn.ID = DataID.RemoveEntity;
                    unspawn.UID = Client.Entity.UID;
                    SendRangePacket.Add(Client.Entity, Kernel.ViewDistance, Client.Entity.UID, Kernel.ToBytes(&unspawn), ConquerCallbackKernel.CommonRemoveScreen);

                    if (Client.InTeam)
                    {
                        if (Client.Team.Leader)
                            Client.Team.DismissTeam(null);
                        else
                            Client.Team.LeaveTeam(null);
                    }
                    if (Client.IsVendor)
                        Client.Vendor.StopVending();
                    if (Client.InTrade)
                        Client.Trade.CloseTrade(null);
                    if (Client.Pet != null)
                        Client.Pet.Kill(null, TIME.Now);

                    Client.AutoAttackEntity = null;

                    CreateUIDCallback.Add(Client.Entity, Client.Friends.GetOnlineUIDList(), ConquerCallbackKernel.NotifyFriendsImOffline);
                    // same shit for enemies ^

                    Client.Screen.FullWipe();
                    Kernel.ClientDictionary.Remove(Client.Entity.UID);

                    if ((Client.ServerFlags & ServerFlags.LoadedCharacter) == ServerFlags.LoadedCharacter)
                    {
                        ServerDatabase.SavePlayer(Client);
                        ServerDatabase.DecPlayerOnline();
                    }
                }
            }
        }

        static unsafe void Game_ReceivePacket(NetworkClient nClient, byte[] Packet)
        {
            if (nClient.Owner != null)
            {
                GameClient Client = nClient.Owner as GameClient;
                Client.Crypto.Decrypt(Packet, Packet, Packet.Length);

                if (Packet.Length <= 4)
                {
                    Client.NetworkSocket.Disconnect();
                    return;
                }

                fixed (byte* lpPacket = Packet)
                {
                    int Counter = 0;
                    byte[] InitialPacket = null;
                    while (Counter < Packet.Length)
                    {
                        ushort Size = (ushort)(*((ushort*)(lpPacket + Counter)));
                        ushort Type = *((ushort*)(lpPacket + Counter + 2));
                        if (Size < Packet.Length)
                        {
                            InitialPacket = new byte[Size];
                            fixed (byte* lpInitialPacket = InitialPacket)
                            {
                                MSVCRT.memcpy(lpInitialPacket, lpPacket + Counter, Size);
                                PacketProcessor.Process(Client, lpInitialPacket, InitialPacket, Type);
                            }
                        }
                        else if (Size > Packet.Length)
                        {
                            nClient.Disconnect();
                            break;
                        }
                        else
                        {
                            PacketProcessor.Process(Client, lpPacket, Packet, Type);
                        }
#if LOG_PACKETS
                        bool OKDump = true;
                        if (Type == 0x3f1)
                        {
                            if (((ItemUsuagePacket*)lpPacket)->ID == ItemUsuageID.Ping)
                                OKDump = false;
                        }
                        if (OKDump)
                            Console.WriteLine(Dump((InitialPacket == null) ? Packet : InitialPacket));
#endif
                        Counter += Size;
                    }
                }
            }
        }
    }
}