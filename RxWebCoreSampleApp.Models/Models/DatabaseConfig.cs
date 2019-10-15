using System.Collections.Generic;

namespace RxWebCoreSampleApp.Models
{
    public class DatabaseConfig
    {
        public Dictionary<string,string> ConnectionString { get; set; }

        public Dictionary<string,int> ConnectionResiliency { get; set; }

        public int CommandTimeout { get; set; }
    }
}
