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
        public string Username { get; set; }

        [DataMember]
        public double WorkHoursMillis { get; set; }

        [DataMember]
        public Mean WorkHours { get; set; }
        [DataMember]

        public string FullDate { get; set; }
    }

}
