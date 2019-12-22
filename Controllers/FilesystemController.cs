using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database.Models;
using EsxiRestfulApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EsxiRestfulApi.Controllers
{
    [ApiController]
    [Route("/filesystems")]
    public class FilesystemController : ControllerBase
    {
        private readonly IFilesystemService _filesystemService;

        public FilesystemController(IFilesystemService filesystemService)
        {
            _filesystemService = filesystemService;
        }

        public async Task<IActionResult> GetList()
        {
            List<Filesystem> filesystems = await _filesystemService.GetAll();

            if (filesystems.Count == 0)
            {
                return Ok(new {});
            }

            return Ok(filesystems);
        }

    }
}