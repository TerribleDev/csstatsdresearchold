using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StatsdClient
{
    internal sealed class UdpOutputChannel : IOutputChannel
    {
        private readonly UdpClient _udpClient = new UdpClient();
        private IPEndPoint _ipEndpoint;
        private string _hostOrIPAddress;
        private int _port;
        public Socket ClientSocket
        {
            get
            {
                return _udpClient.Client;
            }
        }

        public UdpOutputChannel(string hostOrIPAddress, int port)
        {
            _hostOrIPAddress = hostOrIPAddress;
            _port = port;
        }

        private async Task<IPEndPoint> GetIpAddress()
        {
            if(_ipEndpoint != null)
            {
                return _ipEndpoint;
            }
            IPAddress ipAddress;
            // Is this an IP address already?
            if(!IPAddress.TryParse(_hostOrIPAddress, out ipAddress))
            {
                try
                {
                    ipAddress = (await Dns.GetHostAddressesAsync(_hostOrIPAddress).ConfigureAwait(false)).First(p => p.AddressFamily == AddressFamily.InterNetwork);
                }
                catch(Exception)
                {
                    System.Diagnostics.Trace.TraceError("Failed to retrieve domain {0}", _hostOrIPAddress);
                    _ipEndpoint = null;
                    return null;
                }
                
                // Convert to ipv4 address

            }
            _ipEndpoint = new IPEndPoint(ipAddress, _port);
            return _ipEndpoint;
            
            
        }

        public async Task SendAsync(string line)
        {
            var endpoint = await GetIpAddress().ConfigureAwait(false);
            if(endpoint == null) return;
            var payload = Encoding.UTF8.GetBytes(line);
            await _udpClient.SendAsync(payload, payload.Length, endpoint).ConfigureAwait(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    _udpClient.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}