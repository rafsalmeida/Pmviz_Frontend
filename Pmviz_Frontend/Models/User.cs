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

       /* [DataMember]
        public int Id_Role { get; set; }*/

        [DataMember]
        [JsonProperty("password")]
        public string Password { get; set; }
/*
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Email { get; set; }*/
    }
}
