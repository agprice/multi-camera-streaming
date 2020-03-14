
using System.Threading.Tasks;

namespace client.Interfaces.Display {
    public interface IDisplay {
        Task startDisplay();
        void closeDisplay();
    }
}