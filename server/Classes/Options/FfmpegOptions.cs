using server.Interfaces.Options;

namespace server.Classes.Options
{
    public class FfmpegOptions : IOptions
    {
        private string currentOptions = "";

        public FfmpegOptions(string options)
        {
            currentOptions = options;
        }

        public void add(string options)
        {
            options += options;
        }

        public string getOptions()
        {
            return currentOptions;
        }

        public void remove(string options)
        {
            int index = currentOptions.IndexOf(options);
            currentOptions = (index < 0) ? currentOptions : currentOptions.Remove(index, options.Length);
        }
    }
}