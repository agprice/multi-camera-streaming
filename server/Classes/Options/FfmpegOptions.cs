using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

using NLog;
using Microsoft.Extensions.Configuration;

using server.Interfaces.Options;
using server.Classes.Constants;


namespace server.Classes.Options
{
    public class FfmpegOptions : IOptions
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly OrderedDictionary _currentOptions = new OrderedDictionary();

        public FfmpegOptions()
        {
            // Create the builder from json
            var builder = new ConfigurationBuilder().AddJsonFile(ConfigRuntimeConstants.SETTINGS_FILE, optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            // Get all ffmpeg option sections as enumerables
            var options = configuration.GetSection(ConfigRuntimeConstants.FFMPEG).GetChildren().ToList();
            // Get all global defaults as list of enumerables
            var globalDefaults = options.Find(config => config.Key.Equals(ConfigRuntimeConstants.GLOBAL_DEFAULTS)).GetChildren().ToList();
            // Get paralogue global defaults (these are what go before the specific defaults)
            var paralogueGlobal = globalDefaults.Find(config => config.Key.Equals(ConfigRuntimeConstants.PARALOGUE));
            // Get epilogue global defaults (these are what go after the specific defaults)
            var epilogueGlobal = globalDefaults.Find(config => config.Key.Equals(ConfigRuntimeConstants.EPILOGUE));
            // Get all specific OS defaults as a list of enumerables based on the current OS
            var specificDefaults = options.Find(config => config.Key.Equals(ConfigRuntimeConstants.SPECIFIC_DEFAULTS)).GetChildren().ToList()
                                          .Find(config => config.Key == ConfigRuntimeConstants.OS).GetChildren();
            // Add all the paralogue global defaults to the ordered dictionary
            Logger.Info(paralogueGlobal.Key);
            _currentOptions.Add(paralogueGlobal.Key, paralogueGlobal.Value);
            // Add all the specific defaults
            foreach(var sd in specificDefaults)
            {
                _currentOptions.Add(sd.Key, sd.Value);
            }
            // Add all the epilogue global defaults to the ordered dictionary
            _currentOptions.Add(epilogueGlobal.Key, epilogueGlobal.Value);
        }
        
        /// <summary>
        /// Parse the string options passed in.
        /// </summary>
        /// <param name="options">Set of options split by space</param>
        public void add(string options)
        {
            if(options == null || options.Length == 0) return;
            var parsedOptions = options.Split(' ');

            for(var i = 0; i < parsedOptions.Length; i+=2)
            {
                _currentOptions[parsedOptions[i]] = parsedOptions[i+1];
            }
        }

        public string getOptions()
        {
            var arr = new string[_currentOptions.Values.Count];
            _currentOptions.Values.CopyTo(arr, 0);
            return string.Join(" ", arr);
        }

        public void remove(string option)
        {
            _currentOptions.Remove(option);
        }
    }
}