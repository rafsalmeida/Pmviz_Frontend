using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Nodes
    {
        [DataMember]
        public int Frequency { get; set; }

        [DataMember]
        public Duration MeanDuration { get; set; }

        [DataMember]
        public Duration MedianDuration { get; set; }

        [DataMember]
        public Duration MinDuration { get; set; }

        [DataMember]
        public Duration MaxDuration { get; set; }

        [DataMember]
        public int Node { get; set; }
    }
}
