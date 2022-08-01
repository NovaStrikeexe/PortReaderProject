using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
namespace PortWebSocketServer
{
    class Program
    {


        static void Main(string[] args)
        {
            ConnectionToPortService connectionToPortService = new ConnectionToPortService();
            WritingToPortGeneratedDataService writingToPortGeneratedDataService = new WritingToPortGeneratedDataService();

            IPEndPoint ipPoint = new IPEndPoint(
                IPAddress.Parse(ConfigurationManager.AppSettings.Get("WSAdress")),
                Int32.Parse(ConfigurationManager.AppSettings.Get("WSPort"))
            );
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string message = "";
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                Console.WriteLine("The server is running. Waiting for requests...");
                Socket handler = listenSocket.Accept();
                StringBuilder builder = new StringBuilder();
                int bytes = 18;
                byte[] data = new byte[8];
                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    if (builder.ToString() == "y" || builder.ToString() == "Y")
                    {
                        connectionToPortService.OpenPort();
                        writingToPortGeneratedDataService.serialPortOpen();
                        writingToPortGeneratedDataService.GeneratRawDataToPort();
                    }
                }
                while (handler.Available > 0);
                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                message = connectionToPortService.GetDataFromPort();
                connectionToPortService.ClosePort();
                writingToPortGeneratedDataService.serialPortClose();
                data = Encoding.ASCII.GetBytes(message);
                handler.Send(data);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
