using System;
using System.Numerics;

namespace EsxiRestfulApi.Database.Models
{
    public class Filesystem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Size { get; set; }

        public string Free { get; set; }

        public string VolumeName { get; set; }

        public string Uuid { get; set; }

        public bool Mounted { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}