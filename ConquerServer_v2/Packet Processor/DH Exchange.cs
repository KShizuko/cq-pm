using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Client;
using ConquerServer_v2.Packet_Structures;

namespace ConquerServer_v2.Packet_Processor
{
    public unsafe partial class PacketProcessor
    {
        public static void AppendBlowfishLanguage(GameClient Client, byte[] Packet)
        {
            byte[] pubKey;
            if (PacketBuilder.GetPubKeyFromReply(Packet, out pubKey))
            {
                Client.Crypto.DHSetKey(pubKey);
            }
            Client.Crypto.FinishHandShake();
            Client.ServerFlags |= ServerFlags.DHExchanged;
        }
    }
}
