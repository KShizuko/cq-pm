using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Client;
using ConquerServer_v2.Packet_Structures;
using ConquerServer_v2.Database;
using ConquerServer_v2.BruteForce;

namespace ConquerServer_v2
{
    public partial class Program
    {
        static void Auth_ClientConnect(NetworkClient Client)
        {
            Client.Owner = new AuthClient(Client);
            if (BruteforceProtection.IsBanned(Client.IP))
                Client.Disconnect(false);
        }
        static unsafe void Auth_ClientReceive(NetworkClient nClient, byte[] Received)
        {
            AuthClient Client = nClient.Owner as AuthClient;
            Client.Crypto.Decrypt(Received, Received, Received.Length);

            fixed (byte* pReceived = Received)
            {
                ushort Type = *((ushort*)(pReceived + 2));
                switch (Type)
                {
                    case LoginPacket.cType:
                        {
                            LoginPacket* login = (LoginPacket*)pReceived;
                            if (Received.Length == LoginPacket.cSize)
                            {
                                PasswordCrypter.Decrypt((uint*)login->szPassword);
                                Client.Account = login->User;
                                Client.Password = login->Password;

                                Client.AuthID = ServerDatabase.ValidAccount(Client.Account, Client.Password);
                                int PermanentBan = ServerDatabase.PermanentBan(Client.Account);
                                AuthResponsePacket resp = AuthResponsePacket.Create();

                                if (PermanentBan == 2)
                                {
                                    ServerDatabase.AddFullPermanentBan(Client.Account);
                                }
                                else if (PermanentBan == 4)
                                {
                                    ServerDatabase.RemovePermanentBan(Client.Account);
                                }

                                if (Client.AuthID != 0)
                                {
                                    if (PermanentBan == 2)
                                        resp.Type = 0x41E;
                                    else if (PermanentBan == 4)
                                    {
                                        resp.Type = 0x41D;
                                    }
                                    resp.IPAddress = "192.168.1.150";
                                    resp.Key1 = Client.AuthID;
                                    resp.Key2 = (uint)Client.Password.GetHashCode();
                                    resp.Port = 5816;
                                    ServerDatabase.AddAuthData(Client);
                                }
                                else
                                {
                                    resp.Key1 = 1;
                                    BruteforceProtection.AddWatch(nClient.IP);
                                }
                                Client.Send(&resp);
                            }
                            else
                            {
                                nClient.Disconnect(false);
                            }
                            break;
                        }
                }
            }
        }
    }
}
