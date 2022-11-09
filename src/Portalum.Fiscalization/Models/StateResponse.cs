using System;
using System.Text.Json.Serialization;

namespace Portalum.Fiscalization.Models
{
    public class StateResponse
    {
        /// <summary>
        /// Process Id
        /// </summary>
        [JsonPropertyName("Pid")]
        public int ProcessId { get; set; }

        /// <summary>
        /// Architecture
        /// </summary>
        public string Arch { get; set; }

        public int Uptime { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public bool Online { get; set; }

        public string Recorder { get; set; }

        public string Badge { get; set; }
        /// <summary>
        /// EFR ID for cloud communication
        /// </summary>
        [JsonPropertyName("EFR")]
        public string EFR { get; set; }

        [JsonPropertyName("RN")]
        public string RegisterNumbers { get; set; }

        public int RecSent { get; set; }

        public int RecQueued { get; set; }

        public int RetryQueued { get; set; }

        public int TimeOffset { get; set; }

        [JsonPropertyName("D")]
        public DateTime DateTime { get; set; }

        [JsonPropertyName("SC")]
        public string SmartCard { get; set; }

        public float DiskUsage { get; set; }

        public int DiskQuota { get; set; }

        public string Company { get; set; }
    }
}
