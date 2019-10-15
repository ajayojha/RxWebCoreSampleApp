
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RxWebCoreSampleApp.Infrastructure.Singleton
{
    public class UserAccessConfigInfo
    {
        public UserAccessConfigInfo()
        {
            this.AccessInfo = new ConcurrentDictionary<int, Dictionary<int, Dictionary<string, bool>>>();
            this.Tokens = new ConcurrentDictionary<string, string>();
        }
        private ConcurrentDictionary<int, Dictionary<int, Dictionary<string, bool>>> AccessInfo { get; set; }

        public ConcurrentDictionary<string, string> Tokens { get; set; }

        public bool Get(int userId, int applicationModuleId, string action)
        {
            Dictionary<int, Dictionary<string, bool>> moduleIds;
            if (this.AccessInfo.TryGetValue(userId, out moduleIds))
            {
                Dictionary<string, bool> actionAccess;
                if (moduleIds.TryGetValue(applicationModuleId, out actionAccess)) {
                    bool value;
                    if (actionAccess.TryGetValue(action, out value))
                        return value;
                }
            }
            return false;
        }

        public void Save(int userId, Dictionary<int, Dictionary<string, bool>> value)
        {
            this.AccessInfo.AddOrUpdate(userId, value, (x, y) => value);
        }
    }
}


