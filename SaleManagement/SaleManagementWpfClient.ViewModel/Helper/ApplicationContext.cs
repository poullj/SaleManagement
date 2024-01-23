using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagementWpfClient.Helper
{
    public interface IApplicationContext
    {
        string UserName { get; set; }
        string WorkStation { get; set; }
        string EnvironmentName { get; set; }
        string SessionID { get; set; }
    }

    public class ApplicationContext : IApplicationContext
    {
        public string UserName { get; set; }
        public string EnvironmentName { get; set; }
        public string WorkStation { get; set; }
        public string SessionID { get; set; }
    }
}
