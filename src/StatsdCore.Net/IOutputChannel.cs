using System;
using System.Threading.Tasks;

namespace StatsdClient
{
    /// <summary>
    /// Contract for sending raw statds lines to the server
    /// </summary>
    public interface IOutputChannel: IDisposable
    {
        /// <summary>
        /// Sends a line of stats data to the server asynchronously.
        /// </summary>
        Task SendAsync(string line);
    }
}