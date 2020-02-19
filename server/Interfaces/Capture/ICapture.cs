using server.Interfaces.Options;

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
    }
}