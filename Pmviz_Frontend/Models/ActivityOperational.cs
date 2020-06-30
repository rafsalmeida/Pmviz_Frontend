using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class ActivityOperational
    {
        [DataMember]

        public string Activity { get; set; }
        [DataMember]

        public Mean TotalOperationalHours { get; set; }
        [DataMember]

        public double TotalOperationalHoursMillis { get; set; }
        [DataMember]

        public string FullDate { get; set; }
    }
}
