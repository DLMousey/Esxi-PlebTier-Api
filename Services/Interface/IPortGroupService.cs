using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;

namespace EsxiRestfulApi.Services.Interface
{
    public interface IPortGroupService
    {
        public Task<List<PortGroup>> FindAll();
    }
}