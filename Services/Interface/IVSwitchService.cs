using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;

namespace EsxiRestfulApi.Services.Interface
{
    public interface IVSwitchService
    {
        /// <summary>
        /// Retrieve the list of VSwitches in the database
        /// </summary>
        /// <returns></returns>
        public Task<List<VSwitch>> FindAll();

        /// <summary>
        /// Retrieve the list of VSwitches from the ESXI Host
        /// </summary>
        /// <returns></returns>
        public Task<List<VSwitch>> GetAll();
    }
}