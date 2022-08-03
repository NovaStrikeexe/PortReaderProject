using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace PortReaderService.Services
{
    internal class PostmanOfRawDataService
    {
        internal void SendRawDataToWS()
        {
            GeneratRawDataForPortService generatRawDataForPortService = new GeneratRawDataForPortService();
            GetDataFromPortService dataFromPortService = new GetDataFromPortService();
            SerialPort portForWriting = new SerialPort(
                 ConfigurationManager.AppSettings.Get("Port"),
                 9600,
                 Parity.Even,
                 7,
                 StopBits.One
                 );
            SerialPort portForReading = new SerialPort(
                ConfigurationManager.AppSettings.Get("Port2"),
                9600,
                Parity.Even,
                7,
                StopBits.One
                );
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(
                IPAddress.Parse(ConfigurationManager.AppSettings.Get("WSAdress")),
                Int32.Parse(ConfigurationManager.AppSettings.Get("WSPort"))
            );
                portForWriting.Open();
                portForReading.Open();
                portForWriting.WriteLine(generatRawDataForPortService.GeneratRawDataForPort());
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                string message = dataFromPortService.GetDataFromPort(portForReading.ReadExisting());
                portForWriting.Close();
                portForReading.Close();
                if (message != "")
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
                File.AppendAllText(@"C:\Users\phant\Desktop\TestFOLDER\Logs.txt",ex.Message);
            }

        }
    }
}