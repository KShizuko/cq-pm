using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using BlowfishCFB;
using ConquerServer_v2.Packet_Structures;

namespace ConquerServer_v2.Client
{
    public class DHExchange : IDisposable
    {
        [DllImport("gamekeyexchange.dll")]
        private static extern unsafe IntPtr CreateInstance();
        [DllImport("gamekeyexchange.dll")]
        private static extern unsafe void PubKey(byte* pk, IntPtr instance);
        [DllImport("gamekeyexchange.dll")]
        private static extern unsafe void DeleteInstance(IntPtr instance);
        [DllImport("gamekeyexchange.dll")]
        private static extern unsafe void GenerateKey(byte* p, byte* g, IntPtr instance);
        [DllImport("gamekeyexchange.dll")]
        private static extern unsafe void ComputeKey(byte* A, byte* Out, IntPtr instance);

        public static byte[] p;
        public static byte[] g;
        private byte[] pub_key;
        private unsafe IntPtr dhinstance;

        unsafe static DHExchange()
        {
            p = new byte[128];
            fixed (byte* lpP = p)
                MSVCRT.memset(lpP, (byte)'1', 128);
            g = new byte[] { 48, 53 };
        }
        public unsafe DHExchange()
        {
            dhinstance = CreateInstance();
            fixed (byte* lpP = p, lpG = g)
                GenerateKey(lpP, lpG, dhinstance);
            pub_key = null;
        }
        public void Dispose()
        {
            DeleteInstance(dhinstance);
        }
        public unsafe byte[] GetPubKey()
        {
            if (pub_key == null)
            {
                pub_key = new byte[128];
                fixed (byte* lpPub_Key = pub_key)
                    PubKey(lpPub_Key, dhinstance);
            }
            return pub_key;
        }
        public unsafe byte[] SetKey(byte[] pubkey)
        {
            byte[] Out = new byte[64];
            fixed (byte* lpPubKey = pubkey, lpOut = Out)
                ComputeKey(lpPubKey, lpOut, dhinstance);
            return Out;
        }
    }
    public unsafe class BlowfishCrypter
    {
        private static byte[] blowfishKey = ASCIIEncoding.ASCII.GetBytes("DR654dt34trg4UI6");
        private byte[] iv_enc, iv_dec;
        private UnmanagedBlowfishCFB bf;
        private DHExchange DHEx;
        private byte num_dec, num_enc;

        public BlowfishCrypter()
        {
            bf = new UnmanagedBlowfishCFB(blowfishKey);
            DHEx = new DHExchange();
            num_dec = 0;
            num_enc = 0;
            iv_dec = new byte[8];
            iv_enc = new byte[8];
        }
        public unsafe void Encrypt(void* In, byte[] Out, int Size)
        {
            fixed (byte* lpOut = Out, lpIV = iv_enc, lpCounter = &num_enc)
                bf.Encrypt((byte*)In, lpOut, lpIV, Size, lpCounter);
        }
        public unsafe void Encrypt(byte[] In, byte[] Out, int Size)
        {
            fixed (byte* lpIn = In, lpOut = Out, lpIV = iv_enc, lpCounter = &num_enc)
                bf.Encrypt(lpIn, lpOut, lpIV, Size, lpCounter);
        }
        public unsafe void Decrypt(byte[] In, byte[] Out, int Size)
        {
            fixed (byte* lpIn = In, lpOut = Out, lpIV = iv_dec, lpCounter = &num_dec)
                bf.Decrypt(lpIn, lpOut, lpIV, Size, lpCounter);
        }
        public void DHSetKey(byte[] Key)
        {
            bf.SetKey(DHEx.SetKey(Key));
        }
        public void FinishHandShake()
        {
            fixed (byte* lpEnc = iv_enc, lpDec = iv_dec)
            {
                MSVCRT.memset(lpEnc, 0, 8);
                MSVCRT.memset(lpDec, 0, 8);
            }
            num_dec = num_enc = 0;
            DHEx.Dispose();
        }
        public byte[] CreateHandShake()
        {
           return PacketBuilder.HandShakePacket(DHExchange.g, DHExchange.p, DHEx.GetPubKey());
        }
        ~BlowfishCrypter()
        {
            bf.Free();
        }
    }
}
