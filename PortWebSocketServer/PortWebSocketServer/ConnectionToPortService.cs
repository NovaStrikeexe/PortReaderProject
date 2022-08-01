using System;
using System.IO.Ports;
using System.Text.Json;
using System.Configuration;

namespace PortWebSocketServer
{
    class ConnectionToPortService
    {
        SerialPort _serialPort = new SerialPort();

        public void OpenPort()
        {
            _serialPort.PortName = ConfigurationManager.AppSettings.Get("Port");
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 7;
            _serialPort.Parity = Parity.Even;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Open();
        }
        public void ClosePort()
        {
            _serialPort.Close();
        }
        public string GetDataFromPort()//Xunit
        {
            
            string data = _serialPort.ReadExisting();
            var dict = data.Split(new[] { '/' });
            var dataFromPort = new DataFromPort
            {
                dateTime = DateTime.Now,
                operationNumber = Int32.Parse(dict[0]),
                gross = Double.Parse(dict[1]),
                tare = Double.Parse(dict[2]),
                net = Double.Parse(dict[3])
            };
            return JsonSerializer.Serialize(dataFromPort);
        }
    }
}
