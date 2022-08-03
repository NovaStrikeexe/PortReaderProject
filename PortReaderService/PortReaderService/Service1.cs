using System.ServiceProcess;
using System.Threading.Tasks;
using PortReaderService.Services;

namespace PortReaderService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            PostmanOfRawDataService postmanOfRawDataService = new PostmanOfRawDataService();
            while (true)
            {
                //Console.WriteLine(postmanOfRawDataService.SendRawDataToWS());
                postmanOfRawDataService.SendRawDataToWS();
                await Task.Delay(4000);
            }
        }
    }
}
