using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Duration
    {
        [DataMember]
        public int Days { get; set; }
        [DataMember]
        public int Hours { get; set; }

        [DataMember]
        public int Minutes { get; set; }

        [DataMember]
        public int Seconds { get; set; }

        [DataMember]
        public int Millis { get; set; }
    }
}
