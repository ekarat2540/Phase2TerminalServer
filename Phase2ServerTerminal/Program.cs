using System.Net.Sockets;
using System.Net;

class Server
{
    private static IPEndPoint receiverEndpoint;
    static async Task Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5713);
        server.Start();
        Console.WriteLine("Server is started on port 5713.");
        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            _ = Connect(client);
        }
    }
    
    static async Task Connect(TcpClient client)
    {
        
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);
        string message = await reader.ReadLineAsync();

        if (message == "RECEIVER")
        {
            receiverEndpoint = (IPEndPoint)client.Client.RemoteEndPoint;
            writer.WriteLine("RECEIVER_REGISTERED");
            writer.Flush();
            Console.WriteLine($"Receiver Connected: {receiverEndpoint.Address}:{receiverEndpoint.Port}");
        }
        else if (message == "SENDER" && receiverEndpoint != null)
        {
            writer.WriteLine($"{receiverEndpoint.Address}:{receiverEndpoint.Port}");
            writer.Flush();
            Console.WriteLine("Sender Connected and Receiver info sent.");
        }
        reader.Close();
        writer.Close();
        stream.Close();
        client.Close();

    }
}
