
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace client.Interfaces.Display
{
    public interface IDisplay
    {
        Task startDisplay();
        void closeDisplay();
        EventHandler ClosedEvent { get; set; }
    }
}