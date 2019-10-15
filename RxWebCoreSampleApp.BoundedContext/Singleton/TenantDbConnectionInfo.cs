using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RxWebCoreSampleApp.BoundedContext.Singleton
{
    public class TenantDbConnectionInfo
    {

        public TenantDbConnectionInfo() {
            this.ConnectionInfo = new ConcurrentDictionary<int, Dictionary<string,string>>();
        }
        private ConcurrentDictionary<int, Dictionary<string, string>> ConnectionInfo { get; set; }

        public Dictionary<string, string> Get(int name)
        {
            Dictionary<string, string> cacheValue;
            if (this.ConnectionInfo.TryGetValue(name, out cacheValue))
            {
                return cacheValue;
            }
            return null;
        }

        public void Save(int name, Dictionary<string, string> value)
        {
            this.ConnectionInfo.AddOrUpdate(name, value,(x, y) => value);
        }
    }
}

