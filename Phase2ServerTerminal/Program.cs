using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{
    private static IPEndPoint receiverEndPoint;

    static async Task Main(string[] args)
    {
        UdpClient server = new UdpClient(5713);
        Console.WriteLine("Server is started on port 5713.");

        while (true)
        {
            UdpReceiveResult result = await server.ReceiveAsync();
            string message = Encoding.UTF8.GetString(result.Buffer);

            string clientType = message;

            if (clientType == "RECEIVER")
            {
                receiverEndPoint = result.RemoteEndPoint;
                Console.WriteLine($"Receiver Registered: {receiverEndPoint}");
                byte[] response = Encoding.UTF8.GetBytes("RECEIVER_REGISTERED");
                await server.SendAsync(response, response.Length, result.RemoteEndPoint);
            }
            else if (clientType == "SENDER")
            {
                if (receiverEndPoint != null)
                {
                    byte[] response = Encoding.UTF8.GetBytes($"{receiverEndPoint.Address}:{receiverEndPoint.Port}");
                    await server.SendAsync(response, response.Length, result.RemoteEndPoint);
                    Console.WriteLine($"Sent Receiver Info to Sender");
                }
                else
                {
                    byte[] response = Encoding.UTF8.GetBytes("NO_RECEIVER");
                    await server.SendAsync(response, response.Length, result.RemoteEndPoint);
                }
            }
        }
    }
}
