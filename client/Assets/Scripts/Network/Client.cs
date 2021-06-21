using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(PacketManager))]
public class Client : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 50000;
    private TcpClient socket = null;
    private Thread listenThread;
    private PacketManager packetManager;

    private void Start()
    {
        packetManager = GetComponent<PacketManager>();
        Listen();
    }

    public void Listen(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
        Listen();
    }

    public void Listen()
    {
        try {
            listenThread = new Thread(new ThreadStart(ThreadListen));
            listenThread.IsBackground = true;
            listenThread.Start();
        } catch (System.Exception e) {
            Debug.Log("Client connect exception: " + e);
        }
    }

    public void ThreadListen()
    {
        try {
            socket = new TcpClient(ip, port);
            while (true) {
                ReadServerMessage();
            }
        }
        catch (SocketException e) {
            Debug.Log("Socket error: " + e);
        }
    }

    private void ReadServerMessage()
    {
        byte[] byteBuffer = new byte[1024];
        string buffer = "";
        int length;

        using (NetworkStream stream = socket.GetStream()) {
            while ((length = stream.Read(byteBuffer, 0, byteBuffer.Length)) != 0)
                HandleServerMessage(length, ref byteBuffer, ref buffer);
        }
    }

    private void HandleServerMessage(int length, ref byte[] byteBuffer, ref string buffer)
    {
        byte[] data = new byte[length];
        System.Array.Copy(byteBuffer, 0, data, 0, length);
        buffer += Encoding.ASCII.GetString(data);
        int nlIndex = buffer.IndexOf('\n');
        while (nlIndex != -1) {
            string command = buffer.Substring(0, nlIndex);
            if (nlIndex >= buffer.Length - 1)
                buffer = "";
            else {
                buffer = buffer.Substring(nlIndex + 1,
                    buffer.Length - (nlIndex + 1));
            }
            HandleCommand(command);
            nlIndex = buffer.IndexOf('\n');
        }
    }

    public void HandleWelcomeHandshake(string[] args)
    {
        Debug.Log("Connected to server");
        packetManager.SendPacket("GRAPHIC");
    }

    private void HandleCommand(string command)
    {
        if (command.Equals("")) return;
        //if (!packetManager.PacketHasCallback(command))
            Debug.Log("[SRV] " + command);
        packetManager.AddRecievedPacketToQueue(command);
    }

    public void SendPacket(string packet)
    {
        if (socket == null) return;
        try {
            NetworkStream stream = socket.GetStream();
            if (stream.CanWrite) {
                byte[] data = Encoding.ASCII.GetBytes(packet);
                stream.Write(data, 0, data.Length);
            }
        } catch (SocketException e) {
            Debug.Log("Exception on packet send: " + e);
        }
    }
}
