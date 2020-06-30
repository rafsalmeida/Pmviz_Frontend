using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class ResourceStat
    {
        [DataMember]
        public string Resource { get; set; }

        [DataMember]
        public double MeanMillis { get; set; }

        [DataMember]
        public Mean Mean { get; set; }

        [DataMember]
        public double GeneralMean { get; set; }
        [DataMember]

        public string FullDate { get; set; }
    }
}
