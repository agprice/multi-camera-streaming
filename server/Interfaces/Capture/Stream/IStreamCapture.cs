namespace server.Interfaces.Capture.Stream{
    public interface IStreamCapture : ICapture
    {
        /// <summary>
        /// Start streaming video
        /// </summary>
        void startStream();
        
        /// <summary>
        /// Stop streaming video
        /// </summary>
        void stopStream();
    }
}