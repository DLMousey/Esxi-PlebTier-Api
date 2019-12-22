using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;

namespace EsxiRestfulApi.Services.Interface
{
    public interface IVirtualMachineService
    {
        public Task<List<VirtualMachine>> FindAll();

        public Task<List<VirtualMachine>> GetAll();
    }
}