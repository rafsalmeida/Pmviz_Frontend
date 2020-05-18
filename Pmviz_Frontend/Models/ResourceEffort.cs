using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class ResourceEffort
    {
        [DataMember]
        public string Resource { get; set; }

        [DataMember]
        public double EffortMillis { get; set; }

        [DataMember]
        public Mean Effort { get; set; }

    }

}
