using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsxiRestfulApi.Database;
using EsxiRestfulApi.Database.Models;
using EsxiRestfulApi.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EsxiRestfulApi.Services.Implementation
{
    public class VirtualMachineService : IVirtualMachineService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISSHService _sshService;
        private readonly IConfiguration _config;

        public VirtualMachineService(ApplicationDbContext context, ISSHService sshService, IConfiguration config)
        {
            _context = context;
            _sshService = sshService;
            _config = config;
        }

        public async Task<List<VirtualMachine>> FindAll()
        {
            DateTime now = DateTime.Now;
            bool updateRecords = false;
            
            /*
             * Pull the list of Virtual Machines out of the database, check if any of the entries are older
             * than the ageThreshold specified in appsettings.json, if any of the records are older than
             * this max, we'll retrieve a fresh list from the host and update the database.
             */
            List<VirtualMachine> virtualMachines = await _context.VMs.ToListAsync();
            foreach (var vm in virtualMachines)
            {
                var diff = now.Subtract(vm.DateCreated).TotalDays;
                if (diff > _config.GetValue<double>("ESXI:ageThreshold"))
                {
                    updateRecords = true;
                }
            }

            if (updateRecords)
            {
                return await GetAll();
            }

            return virtualMachines;
        }

        public async Task<List<VirtualMachine>> GetAll()
        {
            List<VirtualMachine> virtualMachines = new List<VirtualMachine>();

            var result = _sshService.ExecuteCommand("esxcli vm process list | grep -v -e '^$'").Replace(" ", "");
            string[] entries = result.Split(new[] {'\r', '\n'});

            for (int i = 0; i < entries.Length; i++)
            {
                /*
                 * Every 7 lines we get the name of the VM printed as a header for the subsequent lines which are
                 * all properties of that VM - so instead of doing things properly and parsing the response incase
                 * ESXI decided to surprise us with something, we'll just abuse these known indexes.
                 *
                 * Requires a sanity check that adding 7 to the index isn't going to push us past the ends
                 * of the array otherwise bad things happen.
                 */
                if (i % 7 == 0 && i + 7 <= entries.Length)
                {
                    string vmName = entries[i + 5].Split(":")[1].Trim();
                    
                    VirtualMachine search = await _context.VMs
                        .Where(vm => vm.DisplayName.Equals(vmName))
                        .FirstOrDefaultAsync();

                    if (search == null)
                    {
                        VirtualMachine vm = new VirtualMachine();
                        vm.WorldId = int.Parse(entries[i + 1].Split(":")[1].Trim());
                        vm.ProcessId = int.Parse(entries[i + 2].Split(":")[1].Trim());
                        vm.VMXCartelId = int.Parse(entries[i + 3].Split(":")[1].Trim());
                        vm.Uuid = entries[i + 4].Split(":")[1].Trim();
                        vm.DisplayName = vmName;
                        vm.ConfigFile = entries[i + 6].Split(":")[1].Trim();
                        vm.DateCreated = DateTime.Now;

                        await _context.AddAsync(vm);
                        await _context.SaveChangesAsync();

                        virtualMachines.Add(vm);
                    }
                    else
                    {
                        virtualMachines.Add(search);
                    }
                }
                else
                {
                    continue;
                }
            }
            
            return virtualMachines;
        }
    }
}