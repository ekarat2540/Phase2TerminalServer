using System.Net.Sockets;
using System.Net;

class Server
{
    static async Task Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5713);
        server.Start();
        Console.WriteLine("Server is started on port 5713");
    }
}
