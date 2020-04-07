using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Event
    {
        [DataMember]
        [JsonProperty("Id")]
        public long Id { get; set; }

        [DataMember]
        [JsonProperty("CaseId")]
        public long CaseId { get; set; }

        [DataMember]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("Start")]
        public DateTime Start { get; set; }

        [DataMember]
        [JsonProperty("End")]
        public DateTime End { get; set; }

        [DataMember]
        [JsonProperty("Duration")]
        public long Duration { get; set; }

        [DataMember]
        [JsonProperty("Resource")]
        public string Resource { get; set; }

        [DataMember]
        [JsonProperty("Role")]
        public string Role { get; set; }

        [DataMember]
        [JsonProperty("Id_log")]
        public long Id_log { get; set; }
    }
}
