using System.Collections.Generic;
using System.Threading.Tasks;
using EsxiRestfulApi.Database;
using EsxiRestfulApi.Database.Models;
using EsxiRestfulApi.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EsxiRestfulApi.Services.Implementation
{
    public class PortGroupService : IPortGroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISSHService _sshService;
        private readonly IConfiguration _config;

        public PortGroupService(ApplicationDbContext context, ISSHService sshService, IConfiguration config)
        {
            _context = context;
            _sshService = sshService;
            _config = config;
        }
        
        public async Task<List<PortGroup>> FindAll()
        {
            return await _context.PortGroups.ToListAsync();
        }
    }
}