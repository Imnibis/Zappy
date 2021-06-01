using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 50000;
    private TcpClient socket;
    private Thread listenThread;
    public bool connect = false;

    void Update()
    {
        if (connect) {
            Listen();
            connect = false;
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
            listenThread = new Thread(new ThreadStart(ThreadListen));
            listenThread.IsBackground = true;
            listenThread.Start();
        } catch (System.Exception e) {
            Debug.Log("Client connect exception: " + e);
        }
    }

    private void ThreadListen()
    {
        try {
            socket = new TcpClient(ip, port);
            while (true) {
                ReadServerMessage();
            }
        } catch (SocketException e) {
            Debug.Log("Socket error: " + e);
        }
    }

    private void ReadServerMessage()
    {
        byte[] byteBuffer = new byte[1024];
        string buffer = "";
        using (NetworkStream stream = socket.GetStream()) {
            int length;

            while ((length = stream.Read(byteBuffer, 0, byteBuffer.Length)) != 0) {
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
                    HandleCommand(command, stream);
                    nlIndex = buffer.IndexOf('\n');
                }
            }
        }
    }

    private void HandleCommand(string command, NetworkStream stream)
    {
        if (command.Equals("")) return;
        Debug.Log("[SRV] " + command);
        if (command.Equals("WELCOME")) {
            byte[] data = Encoding.ASCII.GetBytes("GRAPHIC\n");
            stream.Write(data, 0, data.Length);
        }
    }
}
