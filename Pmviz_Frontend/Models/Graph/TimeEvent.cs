using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class TimeEvent
    {
        [DataMember]
        public int Node { get; set; }

        [DataMember]
        public int Frequency { get; set; }
    }
}
