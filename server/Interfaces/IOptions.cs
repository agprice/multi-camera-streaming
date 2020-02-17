using System;

namespace server.Interfaces
{
    public interface IOptions
    {
        /// <summary>
        /// Adds an Option to the set of options
        /// </summary>
        /// <param name="options">Option to add</param>
        void add(IOptions options);
        
        /// <summary>
        /// Remove an Option type from set of options
        /// </summary>
        /// <param name="options">Option to remove</param>
        void remove(IOptions options);
    }
}