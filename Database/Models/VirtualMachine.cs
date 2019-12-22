using System;

namespace EsxiRestfulApi.Database.Models
{
    public class VirtualMachine
    {
        public Guid Id { get; set; }

        public int WorldId { get; set; }

        public int ProcessId { get; set; }

        public int VMXCartelId { get; set; }

        public string Uuid { get; set; }

        public string DisplayName { get; set; }

        public string ConfigFile { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}