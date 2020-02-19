using System;

namespace server.Interfaces.Options
{
    public interface IOptions
    {
        /// <summary>
        /// Adds an Option to the set of options
        /// </summary>
        /// <param name="options">Option to add</param>
        void add(string options);
        
        /// <summary>
        /// Remove an Option type from set of options
        /// </summary>
        /// <param name="options">Option to remove</param>
        void remove(string options);

        /// <summary>
        /// Get options as string formatted options
        /// </summary>
        /// <returns>Returns options created as a String of options</returns>
        string getOptions();
    }
}