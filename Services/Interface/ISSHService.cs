using System.IO;
using System.Threading.Tasks;

namespace EsxiRestfulApi.Services.Interface
{
    public interface ISSHService
    {
        public string ExecuteCommand(string command);
    }
}