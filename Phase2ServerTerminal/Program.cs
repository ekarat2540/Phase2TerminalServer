using System.Net.Sockets;
using System.Net;

class Server
{
    private static IPEndPoint senderEndpoint;
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
        if(message == "SENDER")
        {
            senderEndpoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Console.WriteLine("Sender Connected");
        }else if(message == "RECEIVER")
        {
            writer.WriteLine($"{senderEndpoint.Address}:5714");
            writer.Flush();
            Console.WriteLine("Receiver Connected");
        }
        reader.Close();
        writer.Close();
        stream.Close();
        client.Close();

    }
}
