using System;
using System.Collections.Generic;
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
    public class FilesystemService : IFilesystemService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISSHService _sshService;
        private readonly IConfiguration _config;

        public FilesystemService(ApplicationDbContext context, ISSHService sshService, IConfiguration config)
        {
            _context = context;
            _sshService = sshService;
            _config = config;
        }
        
        public async Task<List<Filesystem>> FindAll()
        {
            DateTime now = DateTime.Now;
            bool updateRecords = false;
            
            /*
             * Pull the list of Filesystems out of the database, check if any of the values are older
             * than the ageThreshold specified in appsettings.json, if any of the records are older than
             * this max, we'll retrieve a fresh list from the host and update the database.
             */
            List<Filesystem> filesystems = await _context.Filesystems.ToListAsync();
            foreach (var filesystem in filesystems)
            {
                var diff = now.Subtract(filesystem.CreatedAt).TotalDays;
                if (diff > _config.GetValue<double>("ESXI:ageThreshold"))
                {
                    updateRecords = true;
                }
            }

            if (updateRecords)
            {
                return await GetAll();
            }

            return filesystems;
        }

        public async Task<List<Filesystem>> GetAll()
        {
            List<Filesystem> filesystems = new List<Filesystem>();

            var result = _sshService.ExecuteCommand("esxcli storage filesystem list");
            foreach (var entry in result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!entry.StartsWith("/vmfs"))
                {
                    continue;
                }
                
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);

                string trimmedEntry = regex.Replace(entry, "|");
                string[] properties = trimmedEntry.Split("|");

                // If we've got less than 6 properties it's probably a system volume and not a user defined one
                // so we'll skip it, don't mess with the system volumes.
                if (properties.Length < 7)
                {
                    continue;
                }

                Filesystem search = await _context.Filesystems
                    .Where(fs => fs.Uuid.Equals(properties[2].Trim())).FirstOrDefaultAsync();

                if (search == null)
                {
                    Filesystem filesystem = new Filesystem
                    {
                        VolumeName = properties[1].Trim(),
                        Uuid = properties[2].Trim(),
                        Mounted = bool.Parse(properties[3].Trim()),
                        Type = properties[4].Trim(),
                        Size = properties[5].Trim(),
                        Free = properties[6].Trim()
                    };

                    await _context.AddAsync(filesystem);
                    await _context.SaveChangesAsync();

                    filesystems.Add(filesystem);
                }
                else
                {
                    filesystems.Add(search);
                }
            }

            return filesystems;
        }
    }
}