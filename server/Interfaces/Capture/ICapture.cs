using server.Interfaces.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using server.Classes.Network;
namespace server.Interfaces.Capture
{
    public interface ICapture
    {
        /// <summary>
        /// Start video capturing/ stream
        /// </summary>
        /// <param name="options">Options to use for video</param>
        void start(IOptions options);

        /// <summary>
        /// Stop video capturing/ stream
        /// </summary>
        void stop();

        /// <summary>
        /// This is the process which will perform the capture.
        /// </summary>
        Stream CaptureStream { get; set; }
    }
}