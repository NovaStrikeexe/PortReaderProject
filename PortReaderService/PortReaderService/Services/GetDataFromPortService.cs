using System;
using System.Text.Json;
using PortReaderService.Models;

namespace PortReaderService.Services
{
    class GetDataFromPortService
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
    }
}
