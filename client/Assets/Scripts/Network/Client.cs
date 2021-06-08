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
    public bool connect = false;

    private void Start()
    {
        packetManager = GetComponent<PacketManager>();
    }

    void Update()
    {
        if (connect) {
            Listen();
            connect = false;
        }
        if (socket != null && socket.Connected) {
            ReadServerMessage();
        }
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
            socket = new TcpClient();
            socket.Connect(ip, port);
        }
        catch (SocketException e) {
            Debug.Log("Socket error: " + e);
        }
    }

    private void ReadServerMessage()
    {
        byte[] byteBuffer = new byte[1024];
        string buffer = "";
        using (NetworkStream stream = socket.GetStream()) {
            stream.ReadAsync(byteBuffer, 0, byteBuffer.Length).ContinueWith(task => HandleServerMessage(task.Result, ref byteBuffer, ref buffer));
            HandleCommandQueue(stream);
        }
    }

    private void HandleServerMessage(int length, ref byte[] byteBuffer, ref string buffer)
    {
        Debug.Log("hsm");
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
        packetManager.SendPacket("GRAPHIC", new string[0]);
    }

    private void HandleCommand(string command)
    {
        if (command.Equals("")) return;
        Debug.Log("[SRV] " + command);
        packetManager.AddRecievedPacketToQueue(command);
    }

    private void HandleCommandQueue(NetworkStream stream)
    {
        lock (packetManager.packetSendQueue) {
            foreach (string packet in packetManager.packetSendQueue) {
                byte[] data = Encoding.ASCII.GetBytes(packet);
                stream.Write(data, 0, data.Length);
            }
            packetManager.packetSendQueue.Clear();
        }
    }
}
