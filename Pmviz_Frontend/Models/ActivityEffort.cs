using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class ActivityEffort
    {
        [DataMember]

        public string Activity { get; set; }
        [DataMember]

        public Mean TotalWorkHours { get; set; }
        [DataMember]

        public double TotalWorkHoursMillis { get; set; }
    }
}
