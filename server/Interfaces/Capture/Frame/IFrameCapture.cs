// Custom packages
using server.Interfaces.Options;

namespace server.Interfaces.Capture.Frame
{
    public interface IFrameCapture : ICapture
    {
        /// <summary>
        /// Capture number of frames given by options
        /// </summary>
        /// <param name="options">Options to use for capturing name</param>
        void CaptureFrames(IOptions options);
    }
}