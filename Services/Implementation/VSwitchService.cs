using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EsxiRestfulApi.Database;
using EsxiRestfulApi.Database.Models;
using EsxiRestfulApi.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EsxiRestfulApi.Services.Implementation
{
    public class VSwitchService : IVSwitchService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISSHService _sshService;
        private readonly IConfiguration _config;

        public VSwitchService(ApplicationDbContext context, ISSHService sshService, IConfiguration config)
        {
            _context = context;
            _sshService = sshService;
            _config = config;
        }
        
        public async Task<List<VSwitch>> FindAll()
        {
            return await _context.VSwitches.ToListAsync();
        }

        public async Task<List<VSwitch>> GetAll()
        {
            List<VSwitch> vSwitches = new List<VSwitch>();
            
            var result = _sshService.ExecuteCommand("esxcli network vswitch standard list");
            string[] entries = Regex.Split(result, @"^vSwitch([0-9]+)$", RegexOptions.Multiline);

            foreach (var str in entries)
            {
                if (!str.StartsWith("\n"))
                {
                    continue;
                }

                string[] properties = str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                
                var currentSwitch = new VSwitch();

                currentSwitch.Name = properties[0].Split(":")[1].Trim();
                currentSwitch.Class = properties[1].Split(":")[1].Trim();
                currentSwitch.NumPorts = int.Parse(properties[2].Split(":")[1].Trim());
                currentSwitch.UsedPorts = int.Parse(properties[3].Split(":")[1].Trim());
                currentSwitch.ConfiguredPorts = int.Parse(properties[4].Split(":")[1].Trim());
                currentSwitch.MTU = int.Parse(properties[5].Split(":")[1].Trim());
                currentSwitch.CDPStatus = properties[6].Split(":")[1].Trim();
                currentSwitch.BeaconEnabled = bool.Parse(properties[7].Split(":")[1].Trim());
                currentSwitch.BeaconInterval = int.Parse(properties[8].Split(":")[1].Trim());
                currentSwitch.BeaconThreshold = int.Parse(properties[9].Split(":")[1].Trim());
                currentSwitch.BeaconRequiredBy = properties[10].Split(":")[1].Trim();
                currentSwitch.Uplinks = properties[11].Split(":")[1].Trim();

                string[] portGroups = properties[12].Split(":")[1].Trim().Split(",");
                foreach (var pGroup in portGroups)
                {
                    PortGroup search = await _context.PortGroups
                        .Where(pg => pg.Name.Equals(pGroup))
                        .FirstOrDefaultAsync();

                    if (search == null)
                    {
                        PortGroup portGroup = new PortGroup
                        {
                            Name = pGroup.Trim(),
                            VSwitchId = currentSwitch.Id,
                            VSwitch = currentSwitch,
                            ActiveClients = 0,
                            VLANId = 0
                        };
                        
                        await _context.AddAsync(portGroup);
                    }
                }
                
                vSwitches.Add(currentSwitch);
            }
            
            Dictionary<int, VSwitch> replacements = new Dictionary<int, VSwitch>();
            foreach (var swtch in vSwitches)
            {
                VSwitch search =
                    await _context.VSwitches.Where(vs => vs.Name.Equals(swtch.Name)).SingleOrDefaultAsync();

                if (search == null)
                {
                    await _context.AddAsync(swtch);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    int index = vSwitches.FindIndex(vs => vs.Name.Equals(swtch.Name));
                    replacements.Add(index, search);
                }
            }

            foreach (var replacement in replacements)
            {
                vSwitches[replacement.Key] = replacement.Value;
            }
            
            return vSwitches;
        }
    }
}