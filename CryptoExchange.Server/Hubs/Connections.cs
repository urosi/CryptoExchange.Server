using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoExchange.Server.Hubs
{
    public class Connections
    {
        private readonly HashSet<string> _connections = new HashSet<string>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(string connectionId)
        {
            lock (_connections) {
                string foundValue;
                if (!_connections.TryGetValue(connectionId, out foundValue))
                {
                    _connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections()
        {
            return _connections;
        }

        public void Remove( string connectionId)
        {
            lock (_connections) {
                string foundValue;
                if (!_connections.TryGetValue(connectionId, out foundValue))
                {
                    return;
                }
                _connections.Remove(connectionId);
            }
        }
    }
}
