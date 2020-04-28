using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class User
    {
        [DataMember]
        [JsonProperty("username")]
        public string Username { get; set; }

        [DataMember]
        [JsonProperty("role")]
        public string Role { get; set; }

        [DataMember]
        [JsonProperty("password")]
        public string Password { get; set; }

        [DataMember]
        [JsonProperty("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
