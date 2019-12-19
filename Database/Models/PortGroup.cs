using System;

namespace EsxiRestfulApi.Database.Models
{
    public class PortGroup
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public VSwitch VSwitch { get; set; }

        public Guid VSwitchId { get; set; }

        public int ActiveClients { get; set; }

        public int VLANId { get; set; }
    }
}