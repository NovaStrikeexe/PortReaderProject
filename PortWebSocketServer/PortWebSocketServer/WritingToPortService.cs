using System;
using System.IO.Ports;
using System.Configuration;

namespace PortWebSocketServer
{
    class WritingToPortGeneratedDataService
    {
        SerialPort _serialPort = new SerialPort(
                ConfigurationManager.AppSettings.Get("Port2"),
                9600,
                Parity.Even,
                7,
                StopBits.One
            );
        Random rnd = new Random();
        public void serialPortOpen()
        {
            _serialPort.Open();
            
        }
        public void GeneratRawDataToPort()
        {
            int y = rnd.Next(1, 2000);
            int x = rnd.Next(y, 20000);
            int z = x - y;
            var dataPort = new DataPort
            {
                operationNumber = rnd.Next(1, 1000),
                gross = x,
                tare = y,
                net = z
            };
            _serialPort.Write(dataPort.operationNumber + "/" + dataPort.gross + "/" + dataPort.tare + "/" + dataPort.net);
        }
        public void serialPortClose()
        {
            _serialPort.Close();
        }
    }

    internal class DataPort
    {
        public object operationNumber { get; set; }
        public int gross { get; set; }
        public int tare { get; set; }
        public int net { get; set; }
    }
}
