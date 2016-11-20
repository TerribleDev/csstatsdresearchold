using System;
using System.Threading.Tasks;

namespace StatsdClient
{
    internal sealed class NullOutputChannel : IOutputChannel
    {
        public void Dispose(){}

        public void Send(string line){}

        public async Task SendAsync(string line){}
    }
}