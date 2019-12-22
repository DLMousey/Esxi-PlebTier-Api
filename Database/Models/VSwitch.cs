using System;
using System.Collections.Generic;

namespace EsxiRestfulApi.Database.Models
{
    public class VSwitch
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Class { get; set; }

        public int NumPorts { get; set; }

        public int UsedPorts { get; set; }

        public int ConfiguredPorts { get; set; }

        public int MTU { get; set; }

        public string CDPStatus { get; set; }

        public bool BeaconEnabled { get; set; }

        public int BeaconInterval { get; set; }

        public int BeaconThreshold { get; set; }

        public string BeaconRequiredBy { get; set; }

        public string Uplinks { get; set; }

        public ICollection<PortGroup> PortGroups { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}