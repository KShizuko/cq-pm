﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConquerServer_v2
{
    public class NetworkClient
    {
        private Socket socket;
        private byte[] buffer;
        private NetworkServerSocket server;
        public Socket Socket { get { return socket; } }
        public NetworkServerSocket Server { get { return server; } }
        public object Owner;
        public string IP;

        public NetworkClient(NetworkServerSocket _server, Socket _socket, int buffer_len)
        {
            server = _server;
            socket = _socket;
            buffer = new byte[buffer_len];
            try { IP = (socket.RemoteEndPoint as IPEndPoint).Address.ToString(); } 
            catch (SocketException) {}
        }

        public void BeginReceive()
        {
            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);
            }
            catch (SocketException)
            {
                if (server.OnDisconnect != null)
                    server.OnDisconnect(this);
            }
        }
        private void Receive(IAsyncResult res)
        {
            try
            {
                int len = socket.EndReceive(res);
                if (len > 0)
                {
                    byte[] received = new byte[len];
                    unsafe
                    {
                        fixed (byte* recv_ptr = received, buf_ptr = buffer)
                        {
                            MSVCRT.memcpy(recv_ptr, buf_ptr, len);
                        }
                    }
                    if (server.OnReceive != null)
                    {
                        server.OnReceive(this, received);
                    }

                    BeginReceive();
                }
                else
                {
                    if (server.OnDisconnect != null)
                        server.OnDisconnect(this);
                }
            }
            catch (SocketException)
            {
                if (server.OnDisconnect != null)
                    server.OnDisconnect(this);
            }
        }
        public void Send(byte[] Packet)
        {
            try
            {
                if (socket.Connected)
                {
                    socket.BeginSend(Packet, 0, Packet.Length, SocketFlags.None, new AsyncCallback(EndSend), null);
                }
            }
            catch (SocketException) { }
            if (!socket.Connected && server.OnDisconnect != null)
            {
                server.OnDisconnect(this);
            }
        }
        private void EndSend(IAsyncResult res)
        {
            try
            {
                socket.EndSend(res);
            }
            catch (SocketException) { }
            if (!socket.Connected && server.OnDisconnect != null)
            {
                server.OnDisconnect(this);
            }
        }
        public void Disconnect(bool reuse)
        {
            try
            {
                socket.Disconnect(reuse);
            }
            catch (SocketException)
            {
                if (server.OnDisconnect != null)
                    server.OnDisconnect(this);
            }
        }
    }

    public delegate void NetworkClientConnection(NetworkClient Client);
    public delegate void NetworkClientReceive(NetworkClient Client, byte[] Packet);

    public class NetworkServerSocket
    {
        private Socket server;
        private int m_Port;

        public Socket Socket { get { return server; } }
        public int Port { get { return m_Port; } }

        public NetworkClientConnection OnConnect;
        public NetworkClientReceive OnReceive;
        public NetworkClientConnection OnDisconnect;

        public int ClientBufferSize;

        public NetworkServerSocket()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Prepare(int port, int backlog)
        {
            m_Port = port;
            server.Bind(new IPEndPoint(IPAddress.Any, m_Port));
            server.Listen(backlog);
        }
        public void BeginAccept()
        {
            server.BeginAccept(new AsyncCallback(Accept), null);
        }
        private void Accept(IAsyncResult res)
        {
            Socket client_socket;
            try { client_socket = server.EndAccept(res); }
            catch (SocketException)
            {
                BeginAccept();
                return;
            }

            client_socket.ReceiveBufferSize = ClientBufferSize;
            NetworkClient client = new NetworkClient(this, client_socket, ClientBufferSize);
            if (OnConnect != null)
            {
                OnConnect(client);
            }
            client.BeginReceive();
            BeginAccept();
        }
    }
}
