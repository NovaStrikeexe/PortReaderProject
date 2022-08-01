using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace PortWebSocketServer
{
    class Program
    {

        static int port = 8005;
        static void Main(string[] args)
        {
            ConnectionToPortService connectionToPortService = new ConnectionToPortService();
            WritingToPortService writingToPortService = new WritingToPortService();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string message = "";
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                Console.WriteLine("The server is running. Waiting for requests...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    StringBuilder builder = new StringBuilder();
                    int bytes = 18; 
                    byte[] data = new byte[8];
                    
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        if(builder.ToString() == "y" || builder.ToString() == "Y")
                        {
                            connectionToPortService.OpenPort();
                            writingToPortService.serialPortOpen();
                            writingToPortService.GeneratRawDataToPort();
                        }
                    }
                    while (handler.Available > 0);
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                    message = connectionToPortService.GetDataFromPort();
                    connectionToPortService.ClosePort();
                    writingToPortService.serialPortClose();
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
