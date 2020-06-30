using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class WorkstationEffort
    {
        [DataMember]
        public string WorkstationName { get; set; }

        [DataMember]
        public double OperationalHoursMillis { get; set; }

        [DataMember]
        public Mean OperationalHours { get; set; }
        [DataMember]

        public string FullDate { get; set; }
    }
}
