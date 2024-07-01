using System.Net.Sockets;
using System.Net;

class Server
{
    private static TcpClient receiverClient;
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
            receiverClient = client;
            Console.WriteLine("Receiver Connected");
        }
        else if (message == "SENDER")
        {
            Console.WriteLine("Sender Connected");
            await ForwardMessages(client);
        }
        static async Task ForwardMessages(TcpClient senderClient)
        {
            NetworkStream senderStream = senderClient.GetStream();
            StreamReader senderReader = new StreamReader(senderStream);

            while (true)
            {
                string message = await senderReader.ReadLineAsync();

                if (receiverClient != null)
                {
                    NetworkStream receiverStream = receiverClient.GetStream();
                    StreamWriter receiverWriter = new StreamWriter(receiverStream);
                    receiverWriter.WriteLine(message);
                    receiverWriter.Flush();
                }
            }
        }

    }
}
