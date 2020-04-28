using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class AlphaMiner
    {
        [DataMember]
        public List<string> Nodes { get; set; }

        [DataMember]
        public List<Relation> Relations { get; set; }

        [DataMember]
        public string Info { get; set; }

        [DataMember]
        public List<TimeEvent> StartEvents { get; set; }

        [DataMember]
        public List<TimeEvent> EndEvents { get; set; }

        [DataMember]
        public Statistic Statistics { get; set; }
    }
}
