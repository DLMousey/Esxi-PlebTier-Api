using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;
using EsxiRestfulApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EsxiRestfulApi.Controllers
{
    [ApiController]
    [Route("/vms")]
    public class VirtualMachineController : ControllerBase
    {
        private readonly IVirtualMachineService _vmService;

        public VirtualMachineController(IVirtualMachineService vmService)
        {
            _vmService = vmService;
        }

        public async Task<IActionResult> GetList()
        {
            List<VirtualMachine> virtualMachines = await _vmService.GetAll();

            if (virtualMachines.Count == 0)
            {
                return Ok(new { });
            }

            return Ok(virtualMachines);
        }
    }
}