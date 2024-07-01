using System.Net.Sockets;
using System.Net;

class Server
{
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
        IPEndPoint senderEndpoint;
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        string message = await reader.ReadLineAsync();
        if(message != "SENDER")
        {
            senderEndpoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Console.WriteLine("Sender Connected");
        }
        client.Close();
    }
}
