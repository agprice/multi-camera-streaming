using server.Interfaces.Options;
using System.IO;
using server.Classes.Network;
namespace server.Interfaces.Capture
{
    public interface ICapture
    {
        /// <summary>
        /// Start video capturing/ stream
        /// </summary>
        /// <param name="options">Options to use for video</param>
        void requestStream(IOptions options, NetworkClientConnection client);

        /// <summary>
        /// Stop video capturing/ stream
        /// </summary>
        void stopStreaming(NetworkClientConnection client);

        /// <summary>
        /// This is the process which will perform the capture.
        /// </summary>
        Stream CaptureStream { get; set; }
    }
}