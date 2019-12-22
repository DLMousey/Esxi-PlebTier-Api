using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;

namespace EsxiRestfulApi.Services.Interface
{
    public interface IFilesystemService
    {
        public Task<List<Filesystem>> FindAll();

        public Task<List<Filesystem>> GetAll();
    }
}