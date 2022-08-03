using System;
using System.IO.Ports;
using System.Text.Json;
using System.Configuration;
using PortReaderService.Models;

namespace PortReaderService.Services
{
    class GeneratRawDataForPortService
    {
        public string GetDataFromPort(string msg)
        {
            var dict = msg.Split(new[] { '/' });
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
        public string GeneratRawDataForPort()
        {
            Random rnd = new Random();
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
            return dataPort.operationNumber + "/" + dataPort.gross + "/" + dataPort.tare + "/" + dataPort.net;
        }
    }
}
