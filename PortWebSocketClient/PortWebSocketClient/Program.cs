using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace SocketTcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(
                IPAddress.Parse(ConfigurationManager.AppSettings.Get("WSAdress")),
                Int32.Parse(ConfigurationManager.AppSettings.Get("WSPort"))
            );

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                Console.Write("Check for data from the device: Y/N ");
                string message = Console.ReadLine();
                if (message == "y" || message == "Y")
                {
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    socket.Send(data);
                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine("Server call back: \n" + builder.ToString());
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}