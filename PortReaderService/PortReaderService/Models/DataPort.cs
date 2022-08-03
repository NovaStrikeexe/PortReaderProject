namespace PortReaderService.Models
{
    internal class DataPort
    {
        public int operationNumber { get; set; }
        public double? gross { get; set; }
        public double? tare { get; set; }
        public double? net { get; set; }
    }
}