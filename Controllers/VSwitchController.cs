using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;
using EsxiRestfulApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EsxiRestfulApi.Controllers
{
    [ApiController]
    [Route("/vswitches")]
    public class VSwitchController : ControllerBase
    {
        private readonly IVSwitchService _vSwitchService;

        public VSwitchController(IVSwitchService vSwitchService)
        {
            _vSwitchService = vSwitchService;
        }
        
        public async Task<IActionResult> GetList()
        {
            List<VSwitch> vSwitches = await _vSwitchService.FindAll();

            if (vSwitches.Count == 0)
            {
                return Ok(new {});
            }
            
            return Ok(vSwitches);
        }
    }
}