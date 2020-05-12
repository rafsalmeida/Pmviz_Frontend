using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Mean
    {
        [DataMember]
        public double Days { get; set; }
        [DataMember]
        public double Hours { get; set; }
        [DataMember]
        public double Minutes { get; set; }
        [DataMember]
        public double Seconds { get; set; }
        [DataMember]
        public double Millis { get; set; }
    }
}
