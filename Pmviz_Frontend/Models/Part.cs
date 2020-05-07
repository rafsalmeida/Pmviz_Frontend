using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Part
    {
        [DataMember]
        //[JsonProperty("code")]
        public string Code { get; set; }

        [DataMember]
        //[JsonProperty("description")]
        public string Description { get; set; }

        [DataMember]
        public string TagRfid { get; set; }
    }
}
