using System;

namespace PortWebSocketServer
{
    internal class DataFromPort
    {
        public DateTime dateTime { get; set; }
        public int operationNumber { get; set; }
        public double? gross { get; set; }
        public double? tare { get; set; }
        public double? net { get; set; }
    }
}